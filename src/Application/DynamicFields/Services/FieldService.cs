using Domain;
using ESH.Resources;
using AutoMapper;
using Persistence;
using FluentResults;
using Domain.Constants;
using DynamicFields.Seed;
using DynamicFields.Models;
using ESH.Constant.Announcement;
using DynamicFields.Configs;
using DynamicFields.Constants;
using ESH.ViewModels.Announcement;
using DynamicFields.Abstraction;

using ESH.BuildingBlocks.Localization.Abstraction;
using ESH.BuildingBlocks.Localization.Contract;
using ESH.SeedworkSystem.ViewModel.Localizer;
using ESH.Utilities;

namespace DynamicFields.Services;

public class FieldService : object, IFieldService
{
	#region DI & Constructor

	private IMapper Mapper { get; }
	private IUnitOfWork UnitOfWork { get; }
	private ILanguageService LanguageService { get; }

	public FieldService(IUnitOfWork unitOfWork, ILanguageService languageService, IMapper mapper) : base()
	{
		Mapper = mapper;
		UnitOfWork = unitOfWork;
		LanguageService = languageService;
	}

	#endregion /DI & Constructor

	#region GetByCategoryIdAsync(string? categoryId)

	public async Task<Result<List<FieldResponseViewModel>>> GetByCategoryIdAsync(
		string? categoryId,
		bool? isActive = true,
		CancellationToken cancellationToken = default)
	{
		var result =
			new FluentResults
				.Result<List<FieldResponseViewModel>>();

		if (string.IsNullOrEmpty(categoryId) == true)
		{
			var errorMessage = string.Format(
				ESH.Resources.Messages.RequiredError,
				ESH.Resources.DataDictionary.Category);

			result.WithError(errorMessage);
			return result;
		}

		var category = await UnitOfWork
			.CategoryRepository.FindAsync(categoryId, cancellationToken:cancellationToken);

		if (category is null)
		{
			var errorMessage = string.Format(
				ESH.Resources.Messages.NotFoundError,
				ESH.Resources.DataDictionary.Category);

			result.WithError(errorMessage);
			return result;
		}

		if (result.IsSuccess == true)
		{
			var entities =
				await UnitOfWork.FieldRepository
					.GetByCategoryIdAsync(categoryId, isActive, cancellationToken);

			foreach (var field in entities)
			{
				if (field.FieldType is not null
					&& field.ConfigVersion < field.FieldType.CurrentConfigVersion)
				{
					field.JsonConfig = field.FieldType.JsonConfig;
					field.ConfigVersion = field.FieldType.CurrentConfigVersion;
				}
			}

			var values = Mapper.Map<List<FieldResponseViewModel>>(entities);

			values = values
				.DistinctBy(vm => vm.Id)
				.ToList();

			await LanguageService.LocalizeAsync(
				values,
				subSystem: nameof(Domain.Field),
				x => x.Id,
				applyValue: (vm, text) => vm.Name = text,
				key: Domain.Field.NamePropertyLocalizer, cancellationToken: cancellationToken);

			await LanguageService.LocalizeAsync(
				values,
				subSystem: nameof(Domain.Field),
				x => x.Id,
				applyValue: (vm, text) => vm.Hint = text,
				key: Domain.Field.HintPropertyLocalizer, cancellationToken: cancellationToken);

			await LanguageService.LocalizeAsync(
				values,
				subSystem: nameof(Domain.Field),
				x => x.Id,
				applyValue: (vm, text) => vm.Description = text,
				key: Domain.Field.DescriptionPropertyLocalizer, cancellationToken: cancellationToken);

			result.WithValue(values);
		}

		return result;
	}

	#endregion /GetByCategoryIdAsync(string? categoryId)

	public async Task<Result<List<UiSelectModel>>> GetCustomValuesAsync(string fieldId, bool? isActive)
	{
		var result = new FluentResults.Result<List<UiSelectModel>>();

		var field = await UnitOfWork
			.FieldRepository.FindAsync(fieldId);

		if (field is null)
		{
			result.WithError(ESH.Helpers.ResponseHelper.Response400WithCode(10));
			return result;
		}

		if (field.FieldType!.Code != FieldTypes.CustomValues)
		{
			result.WithError(ESH.Helpers.ResponseHelper.Response400WithCode(10));
			return result;
		}

		if (result.IsSuccess == true)
		{
			var values = await UnitOfWork.FieldMultiValueRepository
				.GetByFieldIdAsync(fieldId: fieldId, isActive: isActive);

			await LanguageService.LocalizeAsync(
				values,
				subSystem: nameof(Domain.FieldMultiValue),
				x => x.ValueId,
				applyValue: (vm, text) => vm.Value = text,
				key: Domain.FieldMultiValue.TextPropertyLocalizer);

			result.WithValue(values);
		}

		return result;
	}

	#region GetFiltersByCategoryIdAsync(string? categoryId)

	public async Task<Result<List<FieldResponseViewModel>>>
		GetFiltersByCategoryIdAsync(string? categoryId, CancellationToken cancellationToken = default)
	{
		var result =
			new FluentResults
				.Result<List<FieldResponseViewModel>>();

		if (string.IsNullOrEmpty(categoryId) == true)
		{
			var errorMessage = string.Format(
				ESH.Resources.Messages.RequiredError,
				ESH.Resources.DataDictionary.Id);
			
			result.WithError(errorMessage);
			return result;
		}

		var category = await UnitOfWork
			.CategoryRepository.FindAsync(id: categoryId, cancellationToken: cancellationToken);

		if (category is null)
		{
			var errorMessage = string.Format(
				ESH.Resources.Messages.NotFoundError,
				ESH.Resources.DataDictionary.Category);

			result.WithError(errorMessage);
			return result;
		}

		var entities =
			await UnitOfWork.FieldRepository
				.GetByCategoryIdAsync(categoryId: categoryId,
						  cancellationToken: cancellationToken);

		foreach (var field in entities)
		{
			if (field.FieldType is not null
				&& field.ConfigVersion < field.FieldType.CurrentConfigVersion)
			{
				field.JsonConfig = field.FieldType.JsonConfig;
				field.ConfigVersion = field.FieldType.CurrentConfigVersion;
			}
		}

		var filtered = entities
			.Where(f => f.FieldType is not null
						&& FieldTypeUseInFilter.Types.Contains(f.FieldType.Code))
			.ToList();

		var values = Mapper.Map<List<FieldResponseViewModel>>(filtered);

		values = values
			.DistinctBy(vm => vm.Id)
			.ToList();

		await LanguageService.LocalizeAsync(
			values,
			subSystem: nameof(Domain.Field),
			x => x.Id,
			applyValue: (vm, text) => vm.Name = text,
			key: Domain.Field.NamePropertyLocalizer, cancellationToken: cancellationToken);

		await LanguageService.LocalizeAsync(
			values,
			subSystem: nameof(Domain.Field),
			x => x.Id,
			applyValue: (vm, text) => vm.Hint = text,
			key: Domain.Field.HintPropertyLocalizer, cancellationToken: cancellationToken);

		await LanguageService.LocalizeAsync(
			values,
			subSystem: nameof(Domain.Field),
			x => x.Id,
			applyValue: (vm, text) => vm.Description = text,
			key: Domain.Field.DescriptionPropertyLocalizer, cancellationToken: cancellationToken);

		result.WithValue(values);

		return result;
	}

	#endregion /GetFiltersByCategoryIdAsync(string? categoryId)

	#region CreatePriceForCategoryAsync(FieldReadyRequestViewModel model)

	public async Task<Result> CreatePriceForCategoryAsync(
		FieldReadyRequestViewModel model,
		CancellationToken cancellationToken = default)
	{
		var result =
			await CreateFieldAsync(
				model: model,
				fieldTypeCode: FieldTypes.Price,
				jsonConfig: SeedJson.Of(config:
					new NumberConfig
					{
						Min = 0,
						Max = 10000000,
						FancyDetection = false
					}
				),
				newVersion: model.HasNewVersion(),
				cancellationToken: cancellationToken
			);

		if (result.IsSuccess == true)
		{
			await UnitOfWork.SaveAsync(cancellationToken);
		}

		return result;
	}

	#endregion /CreatePriceForCategoryAsync(FieldReadyRequestViewModel model)

	#region CreateTitleForCategoryAsync(FieldReadyRequestViewModel model)

	public async Task<Result> CreateTitleForCategoryAsync(
		FieldReadyRequestViewModel model,
		CancellationToken cancellationToken = default)
	{
		var result =
			await CreateFieldAsync(
				model: model,
				fieldTypeCode: FieldTypes.Title,
				jsonConfig:
				SeedJson.Of(config:
					new StringConfig
					{
						MaxLength = model.MaxLength.HasValue == true ? model.MaxLength.Value : 200
					}
				),
				newVersion: model.HasNewVersion(),
				cancellationToken: cancellationToken
			);

		if (result.IsSuccess == true)
		{
			await UnitOfWork.SaveAsync(cancellationToken);
		}

		return result;
	}

	#endregion /CreateTitleForCategoryAsync(FieldReadyRequestViewModel model)

	#region CreateDescriptionForCategoryAsync(FieldReadyRequestViewModel model)

	public async Task<Result> CreateDescriptionForCategoryAsync(
		FieldReadyRequestViewModel model,
		CancellationToken cancellationToken = default)
	{
		var result =
			await CreateFieldAsync(
				model: model,
				fieldTypeCode: FieldTypes.Description,
				jsonConfig:
				SeedJson.Of(config:
					new TextConfig
					{
						MaxLength = model.MaxLength.HasValue == true ? model.MaxLength.Value : 4000
					}
				),
				newVersion: model.HasNewVersion(),
				cancellationToken: cancellationToken
			);

		if (result.IsSuccess == true)
		{
			await UnitOfWork.SaveAsync(cancellationToken);
		}

		return result;
	}

	#endregion /CreateDescriptionForCategoryAsync(FieldReadyRequestViewModel model)

	#region CreateImageFieldForCategoryAsync(FieldReadyRequestViewModel model)

	public async Task<Result> CreateImageFieldForCategoryAsync(
		FieldReadyRequestViewModel model,
		CancellationToken cancellationToken = default)
	{
		var result =
			await CreateFieldAsync(
				model: model,
				fieldTypeCode: FieldTypes.Attachment,
				jsonConfig:
				SeedJson.Of(config:
					new AttachmentConfig
					{
						MaxCount = model.MaxCountAttachment.HasValue == true ? model.MaxCountAttachment.Value : 5,
						MaxSizeMB = model.MaxSizeMBAttachment.HasValue == true ? model.MaxSizeMBAttachment.Value : 10,
						AllowedExtensions = AttachmentExtensions.Images,
					}
				),
				newVersion: model.HasNewVersion(),
				cancellationToken: cancellationToken
			);

		if (result.IsSuccess == true)
		{
			await UnitOfWork.SaveAsync(cancellationToken);
		}

		return result;
	}

	#endregion /CreateAttachmentForCategoryAsync(FieldReadyRequestViewModel model)

	#region CreateLocationFieldForCategoryAsync(FieldReadyRequestViewModel model)

	public async Task<Result> CreateLocationFieldForCategoryAsync(
		FieldReadyRequestViewModel model,
		CancellationToken cancellationToken = default)
	{
		var result =
			await CreateFieldAsync(
				model: model,
				fieldTypeCode: FieldTypes.Location,
				jsonConfig:
				SeedJson.Of(config:
					new LocationConfig()
					{
						AllowMap = true,
						AddressSummary = false,
					}
				),
				newVersion: model.HasNewVersion(),
				cancellationToken: cancellationToken
			);

		if (result.IsSuccess == true)
		{
			await UnitOfWork.SaveAsync(cancellationToken);
		}

		return result;
	}

	#endregion /CreateLocationFieldForCategoryAsync(FieldReadyRequestViewModel model)

	#region CreateRegionFieldForCategoryAsync(FieldReadyRequestViewModel model)

	public async Task<Result> CreateRegionFieldForCategoryAsync(
		FieldReadyRequestViewModel model,
		CancellationToken cancellationToken = default)
	{
		var result =
			await CreateFieldAsync(
				model: model,
				fieldTypeCode: FieldTypes.Region,
				jsonConfig:
				SeedJson.Of(config:
					new MultiValueConfig()
				),
				newVersion: model.HasNewVersion(),
				cancellationToken: cancellationToken
			);

		if (result.IsSuccess == true)
		{
			await UnitOfWork.SaveAsync(cancellationToken);
		}

		return result;
	}

	#endregion /CreateRegionFieldForCategoryAsync(FieldReadyRequestViewModel model)

	#region CreateFieldAsync(FieldReadyRequestViewModel model, string fieldTypeCode, string jsonConfig)

	private async Task<Result> CreateFieldAsync(
		FieldReadyRequestViewModel model,
		string fieldTypeCode, string jsonConfig, bool newVersion,
		CancellationToken cancellationToken = default)
	{
		var result = new Result();

		var category = await UnitOfWork
			.CategoryRepository.FindAsync(model.CategoryId, cancellationToken:cancellationToken);

		if (category is null)
		{
			var errorMessage = string.Format(
				Messages.NotFoundError, DataDictionary.Category);

			result.WithError(errorMessage);
			return result;
		}

		if (category.CategoryType!.IsOther() is false)
		{
			result.WithError(ESH.Resources.Messages.EditRestrictionCategories);
			return result;
		}

		var fieldType =
			await UnitOfWork
				.FieldTypeRepository
				.GetByCodeAsync(fieldTypeCode, cancellationToken);

		if (fieldType is null)
		{
			throw new NullReferenceException(nameof(fieldType));
		}

		var field = await UnitOfWork.FieldRepository
			.GetByCodeAndCateogryIdAsync(fieldTypeCode, model.CategoryId, cancellationToken);

		if (field is not null)
		{
			result.WithError(ESH.Resources.Messages.FieldHasExistErrorMessage);
			return result;
		}

		var entity =
			new Field
			{
				IsActive = true,
				IsDeleted = false,

				Ordering = model.Ordering,
				IsRequired = model.IsRequired,

				CategoryId = category.Id,
				FieldTypeId = fieldType.Id,

				JsonConfig = jsonConfig
			};

		if (newVersion == true)
		{
			entity.ConfigVersion = 2;
		}
		else
		{
			entity.ConfigVersion = 1;
		}

		if (model.UseDefaultNames is true)
		{
			var dataAllCategoryType = new List<FieldSeedModel>();

			var seedDataOther = new FieldOtherSeedData();
			var seedDataProperty = new FieldPropertySeedData();
			var seedDataPlate = new FieldPlateSeedData();
			var seedDataPhone = new FieldPhoneSeedData();

			dataAllCategoryType.AddRange(seedDataOther.Data);
			dataAllCategoryType.AddRange(seedDataPhone.Data);
			dataAllCategoryType.AddRange(seedDataPlate.Data);
			dataAllCategoryType.AddRange(seedDataProperty.Data);

			var data =
				dataAllCategoryType
					.Where(x => x.Code == fieldTypeCode)
					.FirstOrDefault();

			model.Name =
			[
				new ValueLocalizerViewModel(value: data!.TitleEn, languageCode: "en-US"),
				new ValueLocalizerViewModel(value: data.TitleAr, languageCode: "ar-OM")
			];

			model.Hint =
			[
				new ValueLocalizerViewModel(value: data.HintEn, languageCode: "en-US"),
				new ValueLocalizerViewModel(value: data.HintAr, languageCode: "ar-OM")
			];

			model.FieldDescription =
			[
				new ValueLocalizerViewModel(value: data.DescriptionEn, languageCode: "en-US"),
				new ValueLocalizerViewModel(value: data.DescriptionAr, languageCode: "ar-OM")
			];
		}

		var resultName = await SaveInLocalizersAsync(
			values: model.Name,
			propertyName: Domain.Field.NamePropertyLocalizer,
			relationId: entity.Id,
			cancellationToken: cancellationToken
		);

		result.WithErrors(resultName.Errors);

		var resultHint = await SaveInLocalizersAsync(
			values: model.Hint,
			propertyName: Domain.Field.HintPropertyLocalizer,
			relationId: entity.Id,
			cancellationToken: cancellationToken
		);

		result.WithErrors(resultHint.Errors);

		var resultDescription = await SaveInLocalizersAsync(
			values: model.FieldDescription,
			propertyName: Domain.Field.DescriptionPropertyLocalizer,
			relationId: entity.Id,
			cancellationToken: cancellationToken
		);

		result.WithErrors(resultDescription.Errors);

		if (result.IsSuccess == true)
		{
			await UnitOfWork.FieldRepository.AddAsync(entity, cancellationToken);
		}

		return result;
	}

	#endregion /CreateFieldAsync(FieldReadyRequestViewModel model, string fieldTypeCode, string jsonConfig)

	#region UpdateFieldAsync(FieldReadyRequestViewModel model)

	public async Task<Result> UpdateFieldAsync(
		FieldReadyRequestViewModel model, CancellationToken cancellationToken = default)
	{
		var result = new FluentResults.Result();

		if (string.IsNullOrEmpty(model.Id) == true)
		{
			result.WithError(ESH.Helpers.ResponseHelper.Response400WithCode(10));
			return result;
		}

		var entity = await UnitOfWork
			.FieldRepository.FindAsync(model.Id, cancellationToken:cancellationToken);

		if (entity is null)
		{
			var errorMessage = string.Format(
				ESH.Resources.Messages.NotFoundError,
				ESH.Resources.DataDictionary.Field);

			result.WithError(errorMessage);
			return result;
		}

		if (model.UseDefaultNames is true)
		{
			var dataAllCategoryType = new List<FieldSeedModel>();

			var seedDataOther = new FieldOtherSeedData();
			var seedDataProperty = new FieldPropertySeedData();
			var seedDataPlate = new FieldPlateSeedData();
			var seedDataPhone = new FieldPhoneSeedData();

			dataAllCategoryType.AddRange(seedDataOther.Data);
			dataAllCategoryType.AddRange(seedDataPhone.Data);
			dataAllCategoryType.AddRange(seedDataPlate.Data);
			dataAllCategoryType.AddRange(seedDataProperty.Data);

			var data =
				dataAllCategoryType
					.Where(x => x.Code == entity.FieldType!.Code)
					.FirstOrDefault();

			model.Name =
			[
				new ValueLocalizerViewModel(value: data!.TitleEn, languageCode: "en-US"),
				new ValueLocalizerViewModel(value: data.TitleAr, languageCode: "ar-OM")
			];

			model.Hint =
			[
				new ValueLocalizerViewModel(value: data.HintEn, languageCode: "en-US"),
				new ValueLocalizerViewModel(value: data.HintAr, languageCode: "ar-OM")
			];

			model.FieldDescription =
			[
				new ValueLocalizerViewModel(value: data.DescriptionEn, languageCode: "en-US"),
				new ValueLocalizerViewModel(value: data.DescriptionAr, languageCode: "ar-OM")
			];
		}

		var resultName = await SaveInLocalizersAsync(
			values: model.Name,
			propertyName: Domain.Field.NamePropertyLocalizer,
			relationId: entity.Id,
			cancellationToken: cancellationToken
		);

		result.WithErrors(resultName.Errors);

		var resultHint = await SaveInLocalizersAsync(
			values: model.Hint,
			propertyName: Domain.Field.HintPropertyLocalizer,
			relationId: entity.Id,
			cancellationToken: cancellationToken
		);

		result.WithErrors(resultHint.Errors);

		var resultDescription = await SaveInLocalizersAsync(
			values: model.FieldDescription,
			propertyName: Domain.Field.DescriptionPropertyLocalizer,
			relationId: entity.Id,
			cancellationToken: cancellationToken
		);

		result.WithErrors(resultDescription.Errors);

		if (result.IsSuccess == true)
		{
			entity.Ordering = model.Ordering;
			entity.IsRequired = model.IsRequired;

			await UnitOfWork.SaveAsync(cancellationToken);

			var successMessage = string.Format(
				ESH.Resources.Messages.UpdateMessageSuccess,
				ESH.Resources.DataDictionary.Field);

			result.WithSuccess(successMessage);
		}

		return result;
	}

	#endregion /UpdateFieldAsync(FieldReadyRequestViewModel model)

	#region ChangeActivationAsync(string id)

	public async Task<Result> ChangeActivationAsync(
		string id, CancellationToken cancellationToken = default)
	{
		var result = new FluentResults.Result();

		if (string.IsNullOrEmpty(id) == true)
		{
			result.WithError(ESH.Helpers.ResponseHelper.Response400WithCode(10));
			return result;
		}

		var entity = await UnitOfWork
			.FieldRepository.FindAsync(id, cancellationToken:cancellationToken);

		if (entity is null)
		{
			var errorMessage = string.Format(
				ESH.Resources.Messages.NotFoundError,
				ESH.Resources.DataDictionary.Field);

			result.WithError(errorMessage);
			return result;
		}

		if (result.IsSuccess == true)
		{
			entity.IsActive = !entity.IsActive;
			entity.UpdateDateTime = ESH.Utilities.DateTools.DateTimeNow();

			await UnitOfWork.SaveAsync(cancellationToken);

			var successMessage = string.Format(
				ESH.Resources.Messages.UpdateMessageSuccess,
				ESH.Resources.DataDictionary.Field);

			result.WithSuccess(successMessage);
		}

		return result;
	}

	#endregion /ChangeActivationAsync(string id)

	#region SaveInLocalizersAsync(List<ValueLocalizerViewModel> values, string propertyName, string relationId)

	private async Task<FluentResults.Result> SaveInLocalizersAsync(
		List<ValueLocalizerViewModel> values,
		string propertyName, string relationId,
		CancellationToken cancellationToken = default)
	{
		var owner =
			new LocalizationOwner(
				RelationId: relationId,
				SubSystemName: nameof(Domain.Field),
				PropertyName: propertyName
			);

		var nameLocalizers =
			Mapper.Map<List<ValueLocalizer>>(values);

		var result =
			await LanguageService.AddAsync(
				localizationOwner: owner,
				localizers: nameLocalizers,
				cancellationToken: cancellationToken);

		return result;
	}

	#endregion /SaveInLocalizersAsync(List<ValueLocalizerViewModel> values, string propertyName, string relationId)

	#region UpdateInLocalizersAsync(List<ValueLocalizerViewModel> values, string propertyName, string relationId)

	// ReSharper disable once UnusedMember.Local
	private async Task<FluentResults.Result> UpdateInLocalizersAsync(
		List<ValueLocalizerViewModel> values,
		string propertyName, string relationId,
		CancellationToken cancellationToken = default)
	{
		var owner =
			new LocalizationOwner(
				RelationId: relationId,
				SubSystemName: nameof(Domain.Field),
				PropertyName: propertyName
			);

		var nameLocalizers =
			Mapper.Map<List<ValueLocalizer>>(values);

		var result =
			await LanguageService.UpdateAsync(
				localizationOwner: owner,
				localizers: nameLocalizers,
				cancellationToken: cancellationToken);

		return result;
	}

	#endregion /UpdateInLocalizersAsync(List<ValueLocalizerViewModel> values, string propertyName, string relationId)

	#region CreateFieldWithValuesAsync(FieldValuesRequestViewModel model)

	public async Task<FluentResults.Result> CreateFieldWithValuesAsync(
		FieldCustomValuesRequestViewModel model, CancellationToken cancellationToken = default)
	{
		var result = new FluentResults.Result();

		var fieldType = await UnitOfWork
			.FieldTypeRepository.GetByCodeAsync(FieldTypes.CustomValues, cancellationToken);

		if (fieldType is null)
		{
			throw new NullReferenceException(nameof(fieldType));
		}

		var hashValues = model.Values
				.Select(x => ESH.Helpers.LocalizerHashHelper.GenerateHash(x))
				.ToList();

		if (hashValues.Count != hashValues.Distinct().Count())
		{
			result.WithError(ESH.Resources.Messages.DuplicateValuesNotAllowedErrorMessage);
			return result;
		}

		var field =
			new Field
			{
				IsActive = true,
				IsDeleted = false,

				ConfigVersion = 1,
				CategoryId = model.CategoryId,

				Ordering = model.Ordering,
				FieldTypeId = fieldType.Id,
				IsRequired = model.IsRequired,
			};

		await UnitOfWork.FieldRepository.AddAsync(field, cancellationToken);

		if (model.Name.Any() == true)
		{
			var resultName = await SaveInLocalizersAsync(
				values: model.Name,
				propertyName: Field.NamePropertyLocalizer,
				relationId: field.Id,
				cancellationToken: cancellationToken
			);
		
			result.WithErrors(resultName.Errors);
		}


		if (model.Hint.Any() == true)
		{
			var resultHint = await SaveInLocalizersAsync(
				values: model.Hint,
				propertyName: Field.HintPropertyLocalizer,
				relationId: field.Id,
				cancellationToken: cancellationToken
			);

			result.WithErrors(resultHint.Errors);
		}

		if (model.Name.Any() == true)
		{
			var resultDescription = await SaveInLocalizersAsync(
				values: model.FieldDescription,
				propertyName: Field.DescriptionPropertyLocalizer,
				relationId: field.Id,
				cancellationToken: cancellationToken
			);

			result.WithErrors(resultDescription.Errors);
		}

		foreach (var item in model.Values)
		{
			var fieldMultiValue =
				new FieldMultiValue
				{
					IsActive = true,
					IsDeleted = false,
					Ordering = 100_000,

					FieldId = field.Id,

					Key = ESH.Helpers.LocalizerHashHelper.GenerateHash(item)
				};

			foreach (var valueType in item)
			{
				fieldMultiValue.Description += $" {valueType.Value}";
			}

			fieldMultiValue.Description =
				fieldMultiValue.Description?.Trim();

			await UnitOfWork.FieldMultiValueRepository.AddAsync(fieldMultiValue, cancellationToken);

			var owner =
				new LocalizationOwner(
					RelationId: fieldMultiValue.Id,
					SubSystemName: nameof(Domain.FieldMultiValue),
					PropertyName: Domain.FieldMultiValue.TextPropertyLocalizer
				);

			var valueLocalizer =
				Mapper.Map<List<ValueLocalizer>>(item);

			var resultLocalizers =
				await LanguageService.AddAsync(
					localizationOwner: owner,
					localizers: valueLocalizer,
					cancellationToken: cancellationToken);

			if (resultLocalizers.IsSuccess == false)
			{
				result.WithErrors(resultLocalizers.Errors);
				return result;
			}
		}

		if (result.IsSuccess == true)
		{
			await UnitOfWork.SaveAsync(cancellationToken);

			var successMessage =
				string.Format(
					ESH.Resources.Messages.CreateSuccessMessage,
					ESH.Resources.DataDictionary.Field);

			result.WithSuccess(successMessage);
		}

		return result;
	}

	#endregion /CreateFieldWithValuesAsync(FieldValuesRequestViewModel model)
}

