using Domain;
using ESH.Helpers;
using AutoMapper;
using Persistence;
using FluentResults;
using System.Text.Json;
using Domain.Constants;
using ESH.ViewModels.Shared;
using DynamicFields.Seed;

using ESH.Constant.Announcement;
using DynamicFields.Configs;
using ESH.ViewModels.Announcement;
using DynamicFields.Validator;
using DynamicFields.Abstraction;
using OnlineTranslate.Abstraction;
using ESH.ViewModels.Announcement.MapApp;
using ESH.ViewModels.Announcement.ModelParameters;

using ESH.Constant.Attachment.Announcement;
using ESH.BuildingBlocks.Application.Abstraction;

using ESH.BuildingBlocks.Attachments.Abstraction;
using ESH.BuildingBlocks.Attachments.Contract;
using ESH.BuildingBlocks.Localization.Abstraction;
using ESH.BuildingBlocks.Localization.Models;
using ESH.BuildingBlocks.Logging.Contracts;
using ESH.BuildingBlocks.RequestFeatures;
using ESH.BuildingBlocks.SubSystem.Contract;
using ESH.SeedworkSystem.Domain.Attachment;
using ESH.SeedworkSystem.Domain.Log;
using ESH.ViewModels.Abstraction;

namespace DynamicFields.Services;

public class AnnouncementService : object, IAnnouncementService
{
	#region DI & Constructor

	private IMapper Mapper { get; }
	private IUnitOfWork UnitOfWork { get; }
	private ILanguageService LanguageService { get; }
	private ITranslateService TranslateService { get; }
	private ILogServerManager LogServerManager { get; }
	private ISubSystemManager SubSystemManager { get; }
	private IJwtTokenValidator JwtTokenValidator { get; }
	private IAttachmentService AttachmentService { get; }
	private IAttachmentManager AttachmentManager { get; }
	private ILanguageCodeManager LanguageCodeManager { get; }
	private FieldValidatorFactory FieldValidatorFactory { get; }
	private IAttachmentSubjectManager AttachmentSubjectManager { get; }
	private ILanguageLocalizerManager LanguageLocalizerManager { get; }
	private INotificationAnnouncementService NotificationAnnouncementService { get; }

	public AnnouncementService(
		IMapper mapper,
		IUnitOfWork unitOfWork,
		ILanguageService languageService,
		ITranslateService translateService,
		IJwtTokenValidator jwtTokenValidator,
		ILanguageCodeManager languageCodeManager,
		ILanguageLocalizerManager languageLocalizerManager,
		FieldValidatorFactory fieldValidatorFactory,
		ILogServerManager logServerManager,
		IAttachmentService attachmentService,
		IAttachmentManager attachmentManager,
		IAttachmentSubjectManager attachmentSubjectManager,
		ISubSystemManager subSystemManager,
		INotificationAnnouncementService notificationAnnouncementService
	) : base()
	{
		Mapper = mapper;
		UnitOfWork = unitOfWork;
		LanguageService = languageService;
		TranslateService = translateService;
		LogServerManager = logServerManager;
		AttachmentService = attachmentService;
		AttachmentManager = attachmentManager;
		SubSystemManager = subSystemManager;
		JwtTokenValidator = jwtTokenValidator;
		LanguageCodeManager = languageCodeManager;
		LanguageLocalizerManager = languageLocalizerManager;
		AttachmentSubjectManager = attachmentSubjectManager;
		NotificationAnnouncementService = notificationAnnouncementService;
		FieldValidatorFactory = fieldValidatorFactory ?? throw new ArgumentNullException(nameof(fieldValidatorFactory));
	}

	#endregion / DI & Constructor

	#region GetAllWithPageAsync(AnnouncementParameters parameters)

	public async Task<Result<PagedListResult<AnnouncementMiniResponseViewModel>>> GetAllWithPageAsync(
		AnnouncementParameters parameters,
		CancellationToken cancellationToken = default)
	{
		var result = new Result<PagedListResult<AnnouncementMiniResponseViewModel>>();

		var entities =
			await UnitOfWork.AnnouncementRepository
				.GetAllWithPageAsync(parameters, cancellationToken);

		var announcementIds =
			entities.Select(x => x.Id).ToList();

		var listModels =
			new List<AnnouncementMiniResponseViewModel>();

		var valuesPack = new PagedListResult
			<AnnouncementMiniResponseViewModel>(listModels, entities.MetaData);

		await BuildMiniModelsAsync(entities.ToList()!, announcementIds, listModels, cancellationToken);

		result.WithValue(valuesPack);

		return result;
	}

	#endregion /GetAllWithPageAsync(AnnouncementParameters parameters)
	
	#region GetAlInlListAsync(AnnouncementParameters parameters)

	public async Task<Result<List<AnnouncementMiniResponseViewModel>>> GetAllInListAsync(
		AnnouncementParameters parameters,
		CancellationToken cancellationToken = default)
	{
		var result = new Result<List<AnnouncementMiniResponseViewModel>>();

		var entities =
			await UnitOfWork.AnnouncementRepository
				.GetAllInListAsync(parameters, cancellationToken);

		var announcementIds =
			entities.Select(x => x.Id).ToList();

		var listModels =
			new List<AnnouncementMiniResponseViewModel>();
		
		await BuildMiniModelsAsync(entities.ToList()!, announcementIds, listModels, cancellationToken);

		result.WithValue(listModels);

		return result;
	}

	#endregion /GetAlInlListAsync(AnnouncementParameters parameters)	

	#region ResentVisitAsync(AnnouncementParameters parameters)
	
	public async Task<Result<List<AnnouncementMiniResponseViewModel>>> ResentVisitAsync(
		AnnouncementParameters parameters,
		CancellationToken cancellationToken = default)
	{
		var result = new Result<List<AnnouncementMiniResponseViewModel>>();

		var resultGetAll =
			await GetAllWithPageAsync(parameters, cancellationToken);

		result.WithErrors(resultGetAll.Errors);

		if (result.IsSuccess == true)
		{
			var dictionary =
				resultGetAll.Value.Data.ToDictionary(x => x.Id!, x => x);

			// ساخت خروجی با حفظ ترتیب ورودی
			var value =
				parameters.Ids.Select(id => dictionary.GetValueOrDefault(id))
					.Where(x => x != null).ToList();

			result.WithValue(value!);
		}

		return result;
	}
	
	#endregion /ResentVisitAsync(AnnouncementParameters parameters)

	#region GetByIdAsync(string id)

	/// <summary>
	/// دریافت جزئیات یک آگهی با 
	/// </summary>
	/// <param name="id"></param>
	/// <param name="profileId"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<Result<AnnouncementResponseViewModel>> GetByIdAsync(
		string id, string? profileId, bool isAdmin = false, CancellationToken cancellationToken = default)
	{
		var result = new Result<AnnouncementResponseViewModel>();

		var entity =
			await UnitOfWork.AnnouncementRepository
				.GetByIdWithDetailsAsync(id, cancellationToken);

		if (entity is null)
		{
			var errorMessage = string.Format(
				ESH.Resources.Messages.NotFoundError,
				ESH.Resources.DataDictionary.Announcement);

			result.WithError(errorMessage);
			return result;
		}

		if (isAdmin == false)
		{
			if (entity.ProfileId != profileId && entity.IsHidden == true)
			{
				result.WithError(ESH.Resources.ResponseErrors.ForbiddenError403);
				return result;
			}
		}

		var model = Mapper.Map<AnnouncementResponseViewModel>(entity);

		if (string.IsNullOrEmpty(profileId) == false)
		{
			var notes =
				await UnitOfWork.NoteRepository
					.GetByAnnouncementAndProfileAsync(id, profileId, cancellationToken);

			model.Note = notes.FirstOrDefault()?.Text;
			model.NoteId = notes.FirstOrDefault()?.Id;
		}

		if (model.Profile!.ShowProfileInAnnouncement is false)
		{
			model.Profile = null;
		}
		else
		{
			await AttachmentService.AttachAsync
				<MiniProfileResponseViewModel, MiniProfileRequestViewModel>(model.Profile, nameof(Domain.Profile));
		}

		model.UserPhoneNumber = entity.Profile!.FullPhoneNumber;

		await LanguageService.LocalizeAsync(
			model.Fields,
			subSystem: nameof(Domain.Field),
			x => x.FieldId,
			applyValue: (vm, text) => vm.Key = text,
			key: Domain.Field.NamePropertyLocalizer, cancellationToken: cancellationToken);

		await LanguageService.LocalizeAsync(
			[model],
			subSystem: nameof(Domain.Category),
			x => x.CategoryId,
			applyValue: (vm, text) => vm.CategoryDisplayName = text,
			key: Domain.Category.PropertyNameKey, cancellationToken: cancellationToken);


		await AttachmentService.AttachAsync
			<AnnouncementResponseViewModel, AnnouncementRequestViewModel>(model, nameof(Domain.Announcement));

		foreach (var item in model.Fields)
		{
			switch (item.FieldTypeCode)
			{
				case FieldTypes.PlateStatus:
				{
					var value = await LanguageService.GetValueAsync(
						nameof(PlateStatus),
						item.Value!, Domain.PlateStatus.PropertyNameKey, cancellationToken);

					item.Value = value?.Value;
					item.ValueId = value?.RelationId;
					break;
				}
				case FieldTypes.PhoneOperator:
				{
					var value = await LanguageService.GetValueAsync(
						nameof(PhoneOperator),
						item.Value!, Domain.PhoneOperator.NamePropertyLocalizer, cancellationToken);

					item.Value = value?.Value;
					item.ValueId = value?.RelationId;
					break;
				}
				case FieldTypes.Region:
				{
					var value = await LanguageService.GetValueAsync(
						nameof(Region),
						item.Value!, Domain.Region.PropertyNameKey, cancellationToken);

					item.Value = value?.Value;
					item.ValueId = value?.RelationId;
					break;
				}
				case FieldTypes.CustomValues:
				{
					var value = await LanguageService.GetValueAsync(
						nameof(Domain.FieldMultiValue),
						item.Value!, Domain.FieldMultiValue.TextPropertyLocalizer, cancellationToken);

					item.Value = value?.Value;
					item.ValueId = value?.RelationId;
					break;
				}
				case FieldTypes.PlateLetter:
				{
					model.BlurPlateLetters = entity.BlurPlateLetters;
					
					var plateCode = await UnitOfWork
						.PlateCodeRepository.FindAsync(item.Value!, cancellationToken: cancellationToken);
					
					if (entity.BlurPlateLetters.HasValue == true
							&& entity.BlurPlateLetters.Value == true
							&& entity.ProfileId != profileId)
					{
						item.Value = string.Empty.PadLeft(plateCode!.ArOm.Length, '#');
						item.ValueId = null;
					}
					else
					{
						item.Value = $"{plateCode?.ArOm},{plateCode?.EnUs}";
						item.ValueId = plateCode?.Id;
					}
					break;
				}
				case FieldTypes.MultiValue:
				{
					var plateCode = await UnitOfWork
						.PlateCodeRepository.FindAsync(item.Value!, cancellationToken: cancellationToken);

					item.Value = $"{plateCode?.ArOm},{plateCode?.EnUs}";
					item.ValueId = plateCode?.Id;
					break;
				}
				default:
					break;
			}
		}

		result.WithValue(model);

		return result;
	}

	#endregion /GetByIdAsync(string id)

	#region CreateAsync(AnnouncementCreateRequestViewModel request)

	/// <summary>
	/// ایجاد یک آگهی
	/// </summary>
	/// <param name="model"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <exception cref="NullReferenceException"></exception>
	public async Task<Result<AnnouncementResponseViewModel>> CreateAsync(
		AnnouncementRequestViewModel model,
		CancellationToken cancellationToken = default)
	{
		var result =
			new Result<AnnouncementResponseViewModel>();

		Announcement entity =
			Mapper.Map<Announcement>(model);

		var validate = model.Validate();

		result.WithErrors(validate.Errors);

		if (result.IsSuccess == false)
		{
			return result;
		}

		var category =
			await UnitOfWork
				.CategoryRepository.FindAsync(
					model.CategoryId,
					cancellationToken:cancellationToken);

		if (category is null)
		{
			var errorMessage = string.Format(
				ESH.Resources.Messages.NotFoundError,
				ESH.Resources.DataDictionary.Category);

			result.WithError(errorMessage);

			return result;
		}

		if (category.CategoryType!.IsPlate() == true)
		{
			entity.BlurPlateLetters = model.BlurPlateLetters;
		}
		else
		{
			entity.BlurPlateLetters = null;
		}
		
		int statusCode = 10;

		if (category.CategoryType!.Code == CategoryTypes.Plate
		    || category.CategoryType!.Code == CategoryTypes.Phone)
		{
			statusCode = 30;
		}

		var status = await UnitOfWork
			.StatusRepository.FindByCodeAsync(statusCode, cancellationToken);

		if (status is null)
		{
			throw new NullReferenceException(
				"[AnnouncementService -> CreateAsync] Status with code 10 not found." +
				" | .StatusRepository.FindByCodeAsync(statusCode, cancellationToken);");
		}

		var userId =
			JwtTokenValidator.GetUserId();

		if (userId is null)
		{
			throw new NullReferenceException(
				"[AnnouncementService -> CreateAsync] UserId is null. | JwtTokenValidator.GetUserId();");
		}

		var profile =
			await UnitOfWork
				.ProfileRepository
				.FindAsync(userId, cancellationToken:cancellationToken);

		if (profile is null)
		{
			throw new NullReferenceException(
				"[AnnouncementService -> CreateAsync] User not found." +
				" | .ProfileRepository.FindAsync(userId, cancellationToken);");
		}

		var categoryFields =
			await UnitOfWork.FieldRepository
				.GetByCategoryIdAsync(model.CategoryId, isActive: true, cancellationToken: cancellationToken);

		if (categoryFields.Any() == false)
		{
			var errorMessage = string.Format(
				ESH.Resources.Messages.NotFoundError,
				ESH.Resources.DataDictionary.Field);

			result.WithError(errorMessage);
			return result;
		}

		#region CheckRequiredFields

		var requiredFields = categoryFields
			.Where(f => f.IsRequired == true)
			.ToList();

		var attachmentFieldRequired = requiredFields
			.Where(current => current.FieldType!.Code == FieldTypes.Attachment)
			.FirstOrDefault();

		if (attachmentFieldRequired is not null && model.Attachments.Any() == false)
		{
			var currentLanguageCode = CurrentLanguage.Code();
			
			var languageLocalizer =
				await LanguageLocalizerManager
					.FindAsync(
						nameof(Field),
						attachmentFieldRequired.Id,
						currentLanguageCode,
						Field.NamePropertyLocalizer,
						cancellationToken);

			var requiredError = string.Format(
				ESH.Resources.Messages.RequiredError, languageLocalizer!.Value);

			result.WithError(requiredError);
		}

		foreach (var field in requiredFields)
		{
			var currentFieldRequired = model.Fields
				.Where(fv => fv.FieldId == field.Id)
				.FirstOrDefault();

			if (currentFieldRequired?.FieldId == attachmentFieldRequired?.Id)
			{
				continue;
			}

			if (currentFieldRequired is not null
			    && string.IsNullOrEmpty(currentFieldRequired.Value) == true)
			{
				var currentLanguageCode = ESH.Helpers.CurrentLanguage.Code();
				
				var languageLocalizer =
					await LanguageLocalizerManager
						.FindAsync(
							nameof(Field),
							field.Id,
							currentLanguageCode,
							Field.NamePropertyLocalizer,
							cancellationToken);

				var requiredError = string.Format(
					ESH.Resources.Messages.RequiredError, languageLocalizer!.Value);

				result.WithError(requiredError);
			}
		}

		if (result.IsSuccess == false)
		{
			return result;
		}

		#endregion /CheckRequiredFields

		var validatedFieldValues = new List<ValidatedFieldValue>();

		#region DictionaryChecker

		var fieldTypeIds =
			categoryFields
				.Where(x => x.FieldType!.IsGeneralText() == true)
				.Select(x => x.Id)
				.ToList();

		var texts =
			model.Fields
				.Where(x => fieldTypeIds.Contains(x.FieldId))
				.Select(x => x.Value)
				.ToList();

		var hasDic = await UnitOfWork
			.DictionaryCheckerRepository
				.CheckTextsAsync(texts, cancellationToken);


		if (hasDic == true)
		{
				var statusCode10 = await UnitOfWork
					.StatusRepository.FindByCodeAsync(10, cancellationToken);

				if (status is null)
				{
					throw new NullReferenceException(
						"[AnnouncementService -> CreateAsync] Status with code 10 not found." +
						" | .StatusRepository.FindByCodeAsync(statusCode, cancellationToken);");
				}

				entity.SetStatusId(statusCode10!.Id);

				entity.HasWarningDictionaryChecker = true;
		}
		
		#endregion /DictionaryChecker

		foreach (var fieldValue in model.Fields)
		{
			var field = categoryFields
				.Where(f => f.Id == fieldValue.FieldId)
				.FirstOrDefault();

			if (field is null)
			{
				var logError =
					$"[AnnouncementService -> CreateAsync] Field with id" +
					$" {fieldValue.FieldId} not found in category fields for category" +
					$" {model.CategoryId}. | categoryFields.Where(f => f.Id == fieldValue.FieldId).FirstOrDefault();";

				throw new NullReferenceException(logError);
			}

			var validator =
				FieldValidatorFactory.Resolve(field.FieldType!.Code);

			var config =
				DeserializeFieldConfig(field.FieldType.Code, field.JsonConfig);

			if (config is null && field.FieldType.IsCustomValue() == false)
			{
				var configError =
					$"config not found" +
					$" {field.FieldType.Code} - {field.FieldType.DataType}";

				result.WithError(configError);
				continue;
			}

			if (field.FieldType.IsCustomValue() == true)
			{
				var findFieldValue =
					await UnitOfWork.FieldMultiValueRepository
						.GetByFieldIdAsync(fieldId: field.Id, isActive: true, cancellationToken: cancellationToken);

				var fieldValueIds =
					findFieldValue
						.Select(x => x.ValueId).ToList();

				if (fieldValueIds.Contains(fieldValue.Value) == false)
				{
					var errorMessage = string.Format(
						ESH.Resources.ResponseErrors.RequestNotValid400);

					result.WithError(errorMessage);
					continue;
				}
			}

			if (field.FieldType.IsLocation() == true)
			{
				var locationValue = JsonSerializer.Deserialize<Location>(fieldValue.Value);

				if (locationValue is null)
				{
					var errorMessage = string.Format(
						ESH.Resources.ResponseErrors.RequestNotValid400);

					result.WithError(errorMessage);
					continue;
				}

				var validationResultLocation =
					await validator.Validate(locationValue, config);

				if (validationResultLocation.IsSuccess == false)
				{
					var errors =
						string.Join(", ", validationResultLocation.Errors.Select(e => e.Message));

					result.WithError($"{errors}");
					continue;
				}

				entity.Latitude = locationValue.Latitude;
				entity.Longitude = locationValue.Longitude;

				var validatedValue =
					new ValidatedFieldValue
					{
						Field = field,
						Value = fieldValue.Value,
						FieldId = fieldValue.FieldId,
					};

				validatedFieldValues.Add(validatedValue);
				continue;
			}

			if (field.FieldType.IsPrice() == true)
			{
				entity.Price = Convert.ToInt32(fieldValue.Value);
			}

			if (field.FieldType.IsAttachment() == true)
			{
				var validationResultAttachment =
					await validator.Validate(model.Attachments, config);

				if (validationResultAttachment.IsSuccess == false)
				{
					var errors =
						string.Join(", ", validationResultAttachment.Errors.Select(e => e.Message));

					result.WithError($"{errors}");
					continue;
				}
				else
				{
					foreach (var formFile in model.Attachments)
					{
						int index = model.Attachments.IndexOf(formFile);

						var uploadPolicy = AttachmentReplacePolicy.ReplaceAll;

						if (index == 0)
						{
							uploadPolicy = AttachmentReplacePolicy.ReplacePrimary;
						}

						var owner =
							new AttachmentOwner(
								nameof(Announcement),
								RelationId: entity.Id,
								Domain.Base.ServerKeyConstant.Key,
								SubjectCode: AnnouncementAttachmentSubjectKeys.AnnouncementImage);

						var resultAttachment = await AttachmentService.UploadAsync(
							formFile, owner, uploadPolicy, cancellationToken);

						result.WithErrors(resultAttachment.Errors);

						if (result.IsSuccess == false)
						{
							return result;
						}
					}

					continue;
				}
			}

			var validationResult =
				await validator.Validate(fieldValue.Value, config);

			if (validationResult.IsSuccess == false)
			{
				var errors =
					string.Join(", ", validationResult.Errors.Select(e => e.Message));

				result.WithError($"{errors}");
			}
			else
			{
				if (field.FieldType.IsGeneralText() == true)
				{
					entity.Description += $"{fieldValue.Value} ";
				}

				var validatedValue =
					new ValidatedFieldValue
					{
						Field = field,
						Value = fieldValue.Value,
						FieldId = fieldValue.FieldId,
					};

				validatedFieldValues.Add(validatedValue);
			}
		}

		if (result.IsSuccess == true)
		{
			// var entity = Mapper.Map<Domain.Announcement>(model);

			var languageCode = ESH.Helpers.CurrentLanguage.Code();

			var language = await LanguageCodeManager
				.FindLanguageByCodeAsync(languageCode, cancellationToken);

			if (language is null)
			{
				throw new NullReferenceException(
					$"[AnnouncementService -> CreateAsync] Language with code {languageCode} not found. | LanguageCodeManager.FindLanguageByCodeAsync(languageCode, cancellationToken);");
			}

			entity.LanguageCodeId = language.Id;

			entity.ProfileId = userId;
			entity.SetStatusId(status.Id);

			await UnitOfWork
				.AnnouncementRepository
				.AddAsync(entity, cancellationToken);

			foreach (var validatedValue in validatedFieldValues)
			{
				var fieldValueEntity = new Domain.FieldValueAnnouncement
				{
					Value = validatedValue.Value,
					FieldId = validatedValue.FieldId,
					AnnouncementId = entity.Id,
				};

				await UnitOfWork
					.FieldValueAnnouncementRepository
					.AddAsync(fieldValueEntity, cancellationToken);
			}

			await UnitOfWork.SaveAsync(cancellationToken);

			var successMessage = string.Format(
				ESH.Resources.Messages.CreateSuccessMessage,
				ESH.Resources.DataDictionary.Announcement);

			result.WithSuccess(successMessage);
		}

		return result;
	}

	#endregion /CreateAsync(AnnouncementCreateRequestViewModel request)

	#region UpdateAsync(AnnouncementUpdateRequestViewModel model, string userId)

	/// <summary>
	/// ویرایش یک آگهی
	/// </summary>
	/// <param name="model"></param>
	/// <param name="userId"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <exception cref="Exception"></exception>
	public async Task<Result> UpdateAsync(
		AnnouncementUpdateRequestViewModel model,
		string userId, CancellationToken cancellationToken = default)
	{
		var result = new Result();

		#region Validate

		if (string.IsNullOrEmpty(userId) == true)
		{
			result.WithError(ResponseHelper.Response400WithCode(10));
			return result;
		}

		if (string.IsNullOrEmpty(model.Id) == true)
		{
			result.WithError(ResponseHelper.Response400WithCode(20));
			return result;
		}

		var announcement = await UnitOfWork
			.AnnouncementRepository.GetByIdWithDetailsAsync(model.Id, cancellationToken);

		if (announcement is null)
		{
			result.WithError(ResponseHelper.Response400WithCode(30));
			return result;
		}

		if (announcement.ProfileId != userId)
		{
			result.WithError(ESH.Resources.ResponseErrors.ForbiddenError403);
			return result;
		}

		if (model.Fields.Any() == false)
		{
			result.WithError(ResponseHelper.Response400WithCode(40));
			return result;
		}

		List<Attachment> attachmentsDatabase = [];

		bool isStaticCategory = false;

		if (announcement.Category!.CategoryType!.IsPlate() == true)
		{
			announcement.BlurPlateLetters = model.BlurPlateLetters;
		}
		else
		{
			announcement.BlurPlateLetters = null;
		}
		
		if (announcement.Category!.CategoryType!.IsPhoneOrPlate() == true)
		{
			model.FileSafeIds = [];
			model.Attachments = [];

			isStaticCategory = true;
		}
		else
		{
			if (model.FileSafeIds.Any() == true)
			{
				var safeAttachments =
					await AttachmentManager
						.FindByIdsAsync(model.FileSafeIds, cancellationToken);

				if (safeAttachments.Count != model.FileSafeIds.Count)
				{
					result.WithError(ResponseHelper.Response400WithCode(50));
					return result;
				}
			}

			var attachmentSubject =
				await AttachmentSubjectManager
					.GetByCodeAsync(AnnouncementAttachmentSubjectKeys.AnnouncementImage, cancellationToken);

			var subSystem =
				await SubSystemManager
					.FindByNameAsync(domain: nameof(Domain.Announcement), cancellationToken);

			attachmentsDatabase =
				await AttachmentManager
					.ListByOwnerAsync(
						subSystemId: subSystem!.Id,
						announcement.Id,
						attachmentSubject!.Id,
						cancellationToken);

			if (attachmentsDatabase.Any() == false && model.FileSafeIds.Any() == true)
			{
				result.WithError(ResponseHelper.Response400WithCode(60));
				return result;
			}
		}

		#endregion /Validate

		var categoryFields = await UnitOfWork
			.FieldRepository.GetByCategoryIdAsync(
				announcement.CategoryId, isActive: true, cancellationToken: cancellationToken);

		if (categoryFields.Any() == false)
		{
			throw new Exception(
				"field for this category not found. |" +
				" .FieldRepository.GetByCategoryIdAsync(announcement.CategoryId, cancellationToken);");
		}

		var fieldValueAnnouncements =
			await UnitOfWork.FieldValueAnnouncementRepository
				.GetByAnnouncementIdAsync(announcement.Id, cancellationToken);

		#region DictionaryChecker

		var fieldValueAnnouncementIds =
			fieldValueAnnouncements
				.Where(x => x.Field!.FieldType!.IsGeneralText() == true)
				.Select(x => x.Id)
				.ToList();

		var texts =
			model.Fields
				.Where(x => fieldValueAnnouncementIds.Contains(x.Id))
				.Select(x => x.NewValue)
				.ToList();

		var hasDic = await UnitOfWork
			.DictionaryCheckerRepository
			.CheckTextsAsync(texts, cancellationToken);
		
		if (hasDic == true)
		{
				var statusCode10 = await UnitOfWork
					.StatusRepository.FindByCodeAsync(10, cancellationToken);

				if (announcement is null)
				{
					throw new NullReferenceException(
						"[AnnouncementService -> UpdateAsync] Status with code 10 not found." +
						" | .StatusRepository.FindByCodeAsync(statusCode, cancellationToken);");
				}

				announcement.SetStatusId(statusCode10!.Id);

				announcement.HasWarningDictionaryChecker = true;
		}
		
		#endregion /DictionaryChecker

		
		foreach (var item in model.Fields)
		{
			var fieldValueAnnouncement =
				fieldValueAnnouncements
					.Where(current => current.Id == item.Id)
					.FirstOrDefault();

			if (fieldValueAnnouncement is null)
			{
				result.WithError(ResponseHelper.Response400WithCode(70));
				return result;
			}

			if (fieldValueAnnouncement.Value == item.NewValue)
			{
				continue;
			}

			var fieldValidator = FieldValidatorFactory
				.Resolve(fieldValueAnnouncement.Field!.FieldType!.Code);

			var config = DeserializeFieldConfig(
				fieldValueAnnouncement.Field.FieldType.Code,
				fieldValueAnnouncement.Field.JsonConfig);

			if (config is null && fieldValueAnnouncement.Field.FieldType.IsCustomValue() == false)
			{
				var configError =
					$"config not found" +
					$" {fieldValueAnnouncement.Field.FieldType.Code}" +
					$" - {fieldValueAnnouncement.Field.JsonConfig}";

				result.WithError(configError);
				continue;
			}

			if (fieldValueAnnouncement.Field!.FieldType.IsCustomValue() == true)
			{
				var findFieldValue =
					await UnitOfWork.FieldMultiValueRepository
						.GetByFieldIdAsync(fieldId: fieldValueAnnouncement.Field.Id,
							isActive: true, cancellationToken: cancellationToken);

				var fieldValueIds =
					findFieldValue
						.Select(x => x.ValueId).ToList();

				if (fieldValueIds.Contains(item.NewValue) == false)
				{
					var errorMessage = string.Format(
						ESH.Resources.ResponseErrors.RequestNotValid400);

					result.WithError(errorMessage);
					continue;
				}
				else
				{
					fieldValueAnnouncement.Value = item.NewValue;
					continue;
				}
			}

			if (fieldValueAnnouncement.Field.FieldType!.IsLocation() == true)
			{
				var locationValue = JsonSerializer.Deserialize<Location>(item.NewValue);

				if (locationValue is null)
				{
					result.WithError(ResponseHelper.Response400WithCode(80));
					continue;
				}

				var validationResultLocation =
					await fieldValidator.Validate(locationValue, config);

				if (validationResultLocation.IsSuccess == false)
				{
					var errors =
						string.Join(", ", validationResultLocation.Errors.Select(e => e.Message));

					result.WithError($"{errors}");
					continue;
				}

				announcement.Latitude = locationValue.Latitude;
				announcement.Longitude = locationValue.Longitude;

				fieldValueAnnouncement.Value = item.NewValue;
				continue;
			}

			if (fieldValueAnnouncement.Field.FieldType!.IsPrice() == true)
			{
				announcement.Price = Convert.ToInt32(item.NewValue);
			}

			var validationResult =
				await fieldValidator
					.Validate(item.NewValue, config);

			if (validationResult.IsSuccess == false)
			{
				var errors =
					string.Join(", ", validationResult.Errors.Select(e => e.Message));

				result.WithError($"{errors}");
			}
			else
			{
				fieldValueAnnouncement.Value = item.NewValue;
			}
		}

		if (announcement.Category.CategoryType.IsPhoneOrPlate() == false)
		{
			foreach (var attachment in attachmentsDatabase)
			{
				if (model.FileSafeIds.Contains(attachment.Id) == true)
				{
					continue;
				}

				var owner = new AttachmentOwner(
					nameof(Domain.Announcement),
					announcement.Id,
					ServerId: Domain.Base.ServerKeyConstant.Key,
					AnnouncementAttachmentSubjectKeys.AnnouncementImage
				);

				var attachmentDeleteResult =
					await AttachmentService
						.DeleteByIdAsync(owner, attachment.Id, cancellationToken);

				result.WithErrors(attachmentDeleteResult.Errors);
			}

			for (int index = 0; index < model.Attachments.Count; index++)
			{
				var formFile =
					model.Attachments[index];

				var owner =
					new AttachmentOwner(
						nameof(Domain.Announcement),
						RelationId: announcement.Id,
						ServerId: Domain.Base.ServerKeyConstant.Key,
						SubjectCode: AnnouncementAttachmentSubjectKeys.AnnouncementImage);

				var resultAttachment =
					await AttachmentService.UploadAsync(
						file: formFile,
						owner: owner,
						replacePolicy: AttachmentReplacePolicy.Append,
						cancellationToken: cancellationToken);

				result.WithErrors(resultAttachment.Errors);

				if (result.IsSuccess == false)
				{
					return result;
				}
			}
		}

		if (result.IsSuccess == true)
		{
			var status10 = await UnitOfWork
				.StatusRepository.FindByCodeAsync(10, cancellationToken);

			var status30 = await UnitOfWork
				.StatusRepository.FindByCodeAsync(30, cancellationToken);

			if (isStaticCategory == true)
			{
				announcement.SetStatusId(status30!.Id);
			}
			else
			{
				announcement.SetStatusId(status10!.Id);
			}

			announcement.UpdateDateTime = ESH.Utilities.DateTools.DateTimeNow();

			await UnitOfWork.SaveAsync(cancellationToken);

			var successMessage = string.Format(
				ESH.Resources.Messages.UpdateMessageSuccess,
				ESH.Resources.DataDictionary.Announcement);

			if (isStaticCategory == true)
			{
				await NotificationAnnouncementService
					.SendNotificationForChangeStatusTo10Async(
						categoryId: announcement.CategoryId, profile: announcement.Profile!, cancellationToken);
			}
			else
			{
				await NotificationAnnouncementService
					.SendNotificationForChangeStatusTo30Async(
						categoryId: announcement.CategoryId, profile: announcement.Profile!, cancellationToken);
			}

			result.WithSuccess(successMessage);
		}

		return result;
	}

	#endregion /UpdateAsync(AnnouncementUpdateRequestViewModel model, string userId)

	#region ChangeIsHiddenAsync(string id, string userId)

	/// <summary>
	/// مخفی کردن یک آگهی برای خود کاربر
	/// </summary>
	/// <param name="id"></param>
	/// <param name="userId"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<Result<bool>> ChangeIsHiddenAsync(
		string id,
		string userId,
		CancellationToken cancellationToken = default)
	{
		var result = new FluentResults.Result<bool>();

		var announcement = await UnitOfWork
			.AnnouncementRepository.FindAsync(id, cancellationToken:cancellationToken);

		if (announcement is null)
		{
			var errorMessage = string.Format(
				ESH.Resources.Messages.NotFoundError,
				ESH.Resources.DataDictionary.Announcement);

			result.WithError(errorMessage);

			return result;
		}

		if (announcement.ProfileId != userId)
		{
			var errorMessage =
				ESH.Resources.ResponseErrors.ForbiddenError403;

			result.WithError(errorMessage);
		}

		if (result.IsSuccess == true)
		{
			announcement.SetIsHidden(!announcement.IsHidden);

			await UnitOfWork.SaveAsync(cancellationToken);

			result.WithValue(announcement.IsHidden);

			result.WithSuccess(ESH.Resources.Messages.AnnouncementHiddenStatusChangedSuccessfully);
		}

		return result;
	}

	#endregion /ChangeIsHiddenAsync(string id, string userId)

	#region ChangeIsActiveAsync(string id)

	/// <summary>
	/// فعال کردن یک آگهی
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<Result<bool>> ChangeIsActiveAsync(
		string id,
		CancellationToken cancellationToken = default)
	{
		var result = new FluentResults.Result<bool>();

		var announcement = await UnitOfWork
			.AnnouncementRepository.FindAsync(id, cancellationToken:cancellationToken);

		if (announcement is null)
		{
			var errorMessage = string.Format(
				ESH.Resources.Messages.NotFoundError,
				ESH.Resources.DataDictionary.Announcement);

			result.WithError(errorMessage);

			return result;
		}

		if (result.IsSuccess == true)
		{
			announcement.SetIsActive(!announcement.IsActive);

			await UnitOfWork.SaveAsync(cancellationToken);

			result.WithValue(announcement.IsActive);

			result.WithSuccess(
				ESH.Resources.Messages.AnnouncementHiddenStatusChangedSuccessfully);
		}

		return result;
	}

	#endregion /ChangeIsActiveAsync(string id)

	#region GetMiniModelsByIdsAsync(List<string> announcementIds)

	public async Task<Result<List<AnnouncementMiniResponseViewModel>>> GetMiniModelsByIdsAsync(
		List<string> announcementIds,
		CancellationToken cancellationToken = default)
	{
		var result = new Result<List<AnnouncementMiniResponseViewModel>>();

		if (announcementIds.Count == 0)
		{
			result.WithValue([]);
			return result;
		}

		var entities =
			await UnitOfWork.AnnouncementRepository
				.GetByIdsAsync(announcementIds, cancellationToken);

		var listModels = new List<AnnouncementMiniResponseViewModel>();

		if (entities.Count == 0)
		{
			result.WithValue(listModels);
			return result;
		}

		await BuildMiniModelsAsync(
			entities!,
			announcementIds,
			listModels, cancellationToken);

		result.WithValue(listModels);

		return result;
	}

	#endregion /GetMiniModelsByIdsAsync(List<string> announcementIds)

	#region GetAnnouncementsWithNotesPopulatedAsync()

	/// <summary>
	/// دریافت لیست آگهی با یادداشت
	/// </summary>
	/// <param name="profileId"></param>
	/// <param name="parameters"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<Result<PagedListResult<AnnouncementMiniResponseViewModel>>>
		GetAnnouncementsWithNotesPopulatedAsync(
			string profileId,
			AnnouncementParameters parameters,
			CancellationToken cancellationToken = default)
	{
		var result = new Result<PagedListResult<AnnouncementMiniResponseViewModel>>();

		if (string.IsNullOrEmpty(profileId))
		{
			result.WithError(ESH.Resources.ResponseErrors.UnauthorizedError401);
			return result;
		}

		var notes = await UnitOfWork.NoteRepository
			.GetByProfileIdAsync(profileId, cancellationToken);

		if (notes.Count == 0)
		{
			result.WithValue(
				new PagedListResult<AnnouncementMiniResponseViewModel>(
					new List<AnnouncementMiniResponseViewModel>()
					, new MetaData()));

			return result;
		}

		var announcementIds = notes
			.Select(n => n.AnnouncementId)
			.Distinct()
			.ToList();

		parameters.Ids = notes.Select(x => x.AnnouncementId).ToList();

		var entities =
			await UnitOfWork.AnnouncementRepository
				.GetAllWithPageAsync(parameters, cancellationToken);

		var listModels =
			new List<AnnouncementMiniResponseViewModel>();

		await BuildMiniModelsAsync(entities.ToList()!, announcementIds, listModels, cancellationToken);

		var values = new PagedListResult
			<AnnouncementMiniResponseViewModel>(listModels, entities.MetaData);

		var noteDictionary = notes
			.GroupBy(n => n.AnnouncementId)
			.ToDictionary(g => g.Key, g => (g.First().Text, g.First().Id));

		foreach (var announcement in values.Data)
		{
			if (noteDictionary.TryGetValue(announcement.Id!, out var note))
			{
				announcement.Note = note.Text;
				announcement.NoteId = note.Id;
			}
		}

		result.WithValue(values);

		return result;
	}

	#endregion /GetAnnouncementsWithNotesPopulatedAsync()

	#region GetAdminDashboardStatsAsync(string? statusId)

	/// <summary>
	/// دریافت آمار مربوط به آگهی ها
	/// </summary>
	/// <param name="statusId">شناسه وضعیت آگهی</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<Result<AdminDashboardStatsViewModel>> GetAdminDashboardStatsAsync(
		string? statusId,
		CancellationToken cancellationToken = default)
	{
		var result = new Result<AdminDashboardStatsViewModel>();

		AdminDashboardStatsViewModel value =
			await UnitOfWork.AnnouncementRepository
				.GetAdminDashboardStatsViewModel(statusId);

		await LanguageService.LocalizeAsync(
			value.TopCategoriesByAnnouncements,
			subSystem: nameof(Domain.Category),
			x => x.Category.Id,
			applyValue: (vm, text) => vm.Category.Name = text,
			key: Domain.Category.PropertyNameKey, cancellationToken: cancellationToken);

		await LanguageService.LocalizeAsync(
			value.TopCategoriesByAnnouncements,
			subSystem: nameof(Domain.Category),
			x => x.Category.ParentId,
			applyValue: (vm, text) => vm.Category.ParentDisplayName = text,
			key: Domain.Category.PropertyNameKey, cancellationToken: cancellationToken);

		var categories =
			value.TopCategoriesByAnnouncements
				.Select(x => x.Category).ToList();

		await AttachmentService.AttachAsync<
				CategoryResponseViewModel, CategoryRequestViewModel>
			(categories, nameof(Domain.Category));

		result.WithValue(value);

		return result;
	}

	#endregion GetAdminDashboardStatsAsync(string? statusId)

	#region GetChartDataForStatusAsync()

	/// <summary>
	/// دریافت دیتای چارت مربوط به فرانت پنل ادمین
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<Result<List<ChartDataViewModel>>>
		GetChartDataForStatusAsync(CancellationToken cancellationToken = default)
	{
		var result = new FluentResults.Result<List<ChartDataViewModel>>();

		var data = await UnitOfWork.StatusRepository
			.GetChartDataForStatusAsync(cancellationToken);

		await LanguageService.LocalizeAsync(
			data,
			subSystem: nameof(Domain.Status),
			x => x.Id,
			applyValue: (vm, text) => vm.Label = text,
			key: Domain.Status.TitleProperty, cancellationToken: cancellationToken);

		result.WithValue(data);

		return result;
	}

	#endregion /GetChartDataForStatusAsync()

	#region AcceptForPublishAsync(string? id, string profileId)

	/// <summary>
	/// تایید برای پابلیش شدن یک آگهی
	/// </summary>
	/// <param name="id"></param>
	/// <param name="profileId"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <exception cref="NullReferenceException"></exception>
	public async Task<Result> AcceptForPublishAsync(
		string? id, string profileId, CancellationToken cancellationToken = default)
	{
		var result = new FluentResults.Result();

		if (string.IsNullOrEmpty(id) == true)
		{
			result.WithError(ESH.Helpers.ResponseHelper.Response400WithCode(10));
			return result;
		}

		var entity = await UnitOfWork
			.AnnouncementRepository.FindAsync(id, cancellationToken:cancellationToken);

		if (entity is null)
		{
			result.WithError(ESH.Helpers.ResponseHelper.Response400WithCode(20));
			return result;
		}

		var status = await UnitOfWork
			.StatusRepository.FindByCodeAsync(AnnouncementStatusCodes.Publish, cancellationToken);

		if (status is null)
		{
			string errorMessage =
				"[AnnouncementService -> AcceptForPublishAsync] Status with code 10 not found. | .StatusRepository.FindByCodeAsync(30);";

			throw new NullReferenceException(errorMessage);
		}

		if (entity.Status!.Code == 30)
		{
			result.WithSuccess(ESH.Resources.Messages.AnnouncementHasPublishedStatus);
			return result;
		}

		if (entity.Status!.Code > 30)
		{
			result.WithError(ESH.Resources.Messages.AnnouncementStatusIsLocked);
			return result;
		}

		if (result.IsSuccess == true)
		{
			entity.SetStatusId(status.Id);
			await UnitOfWork.SaveAsync(cancellationToken);

			await NotificationAnnouncementService
				.SendNotificationForChangeStatusTo30Async(
					categoryId: entity.CategoryId, profile: entity.Profile!, cancellationToken: cancellationToken);

			var successMessage = string.Format(
				ESH.Resources.Messages.UpdateMessageSuccess,
				ESH.Resources.DataDictionary.Announcement);

			result.WithSuccess(successMessage);
		}

		return result;
	}

	#endregion /AcceptForPublishAsync(string? id, string profileId)

	#region ChangeStatusNeedToEditAsync(NeetToEditLogRequestViewModel model, string userId)

	/// <summary>
	/// تغییر وضعیت به نیاز به ویرایش
	/// </summary>
	/// <param name="model"></param>
	/// <param name="userId"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <exception cref="NullReferenceException"></exception>
	public async Task<Result> ChangeStatusNeedToEditAsync(
		NeetToEditLogRequestViewModel model,
		string userId,
		CancellationToken cancellationToken = default)
	{
		var result = new FluentResults.Result();

		var modelValidate = model.Validate();

		result.WithErrors(modelValidate.Errors);

		if (result.IsSuccess is false)
		{
			return result;
		}

		var entity = await UnitOfWork
			.AnnouncementRepository.FindAsync(model.AnnouncementId, cancellationToken:cancellationToken);

		if (entity is null)
		{
			result.WithError(ESH.Resources.ResponseErrors.RequestNotValid400);
			return result;
		}

		var status = await UnitOfWork
			.StatusRepository.FindByCodeAsync(20, cancellationToken);

		if (status is null)
		{
			string errorMessage =
				"[AnnouncementService -> AcceptForPublishAsync] Status with code 10 not found. | .StatusRepository.FindByCodeAsync(30);";

			throw new NullReferenceException(errorMessage);
		}

		var needToEditReason = await UnitOfWork
			.NeedToEditReasonRepository.FindAsync(model.NeedToEditReasonId, cancellationToken:cancellationToken);

		if (needToEditReason is null)
		{
			string errorMessage =
				$"[AnnouncementService -> AcceptForPublishAsync] NeedToEditReason with id {model.NeedToEditReasonId} not found. | .NeedToEditReasonRepository.FindAsync(needToEditReasonId);";

			throw new NullReferenceException(errorMessage);
		}

		//if (entity!.Status!.Code > 10)
		//{
		//	result.WithError(ESH.Resources.Messages.AnnouncementStatusIsLocked);
		//	return result;
		//}

		if (result.IsSuccess == true)
		{
			entity.SetStatusId(status.Id);

			var needToEditLog =
				new Domain.NeedToEditLog
				{
					ProfileId = userId,
					AnnouncementId = entity.Id,
					NeedToEditReasonId = needToEditReason.Id,
				};

			await UnitOfWork
				.NeedToEditLogRepository
				.AddAsync(needToEditLog, cancellationToken);

			await UnitOfWork.SaveAsync(cancellationToken);

			var successMessage = string.Format(
				ESH.Resources.Messages.UpdateMessageSuccess,
				ESH.Resources.DataDictionary.Announcement);

			result.WithSuccess(successMessage);

			await NotificationAnnouncementService
				.SendNotificationForChangeStatusTo20Async(
					categoryId: entity.CategoryId,
					needToEditReasonId: needToEditReason.Id,
					profile: entity.Profile!, cancellationToken: cancellationToken);
		}

		return result;
	}

	#endregion /ChangeStatusNeedToEditAsync(string id, string needToEditReasonId, string userId)

	#region ChangeStatusToRejectedAsync(string id, string userId)

	/// <summary>
	/// تغییر وضعیت به رد شده
	/// </summary>
	/// <param name="id"></param>
	/// <param name="userId"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <exception cref="NullReferenceException"></exception>
	public async Task<Result> ChangeStatusToRejectedAsync(
		string id,
		string userId,
		CancellationToken cancellationToken = default)
	{
		var result = new FluentResults.Result();

		if (string.IsNullOrEmpty(id) == true)
		{
			result.WithError(ESH.Helpers.ResponseHelper.Response400WithCode(10));
		}

		var entity = await UnitOfWork
			.AnnouncementRepository.FindAsync(id, cancellationToken:cancellationToken);

		if (entity is null)
		{
			result.WithError(ESH.Helpers.ResponseHelper.Response400WithCode(20));
			return result;
		}

		var status = await UnitOfWork
			.StatusRepository.FindByCodeAsync(40, cancellationToken);

		if (status is null)
		{
			string errorMessage =
				"[AnnouncementService -> AcceptForPublishAsync]" +
				" Status with code 40 not found. | .StatusRepository.FindByCodeAsync(40);";

			throw new NullReferenceException(errorMessage);
		}

		if (entity.Status!.Code > 20)
		{
			result.WithError(ESH.Resources.Messages.AnnouncementStatusIsLocked);
			return result;
		}

		if (result.IsSuccess == true)
		{
			entity.SetStatusId(status.Id);

			await UnitOfWork.SaveAsync(cancellationToken);

			var successMessage = string.Format(
				ESH.Resources.Messages.UpdateMessageSuccess,
				ESH.Resources.DataDictionary.Announcement);

			result.WithSuccess(successMessage);

			await NotificationAnnouncementService
				.SendNotificationForChangeStatusTo40Async(
					categoryId: entity.CategoryId, profile: entity.Profile!, cancellationToken: cancellationToken);
		}

		return result;
	}

	#endregion /ChangeStatusToRejectedAsync(string id, string needToEditReasonId, string userId)

	#region DeleteAsync(string id, string deleteReasonId, string profileId)

	public async Task<Result> DeleteAsync(
		DeleteLogRequestViewModel model,
		string profileId, CancellationToken cancellationToken = default)
	{
		var result = new FluentResults.Result();

		var modelValidate = model.Validate();

		result.WithErrors(modelValidate.Errors);

		if (result.IsSuccess is false)
		{
			return result;
		}

		var profile = await UnitOfWork
			.ProfileRepository.FindAsync(profileId, cancellationToken:cancellationToken);

		if (profile is null)
		{
			result.WithError(ESH.Resources.ResponseErrors.UnauthorizedError401);
			return result;
		}

		var deleteReason =
			await UnitOfWork
				.DeleteReasonRepository
				.FindAsync(model.DeleteReasonId, cancellationToken:cancellationToken);

		if (deleteReason is null)
		{
			result.WithError(ESH.Resources.ResponseErrors.RequestNotValid400);
			return result;
		}

		if (deleteReason.HasDescription == true
		    && string.IsNullOrEmpty(model.Description) == true)
		{
			result.WithError(ESH.Resources.ResponseErrors.RequestNotValid400);
			return result;
		}

		var entity = await UnitOfWork
			.AnnouncementRepository
			.FindAsync(model.AnnouncementId, cancellationToken:cancellationToken);

		if (entity is null)
		{
			var errorMessage = string.Format(
				ESH.Resources.Messages.NotFoundError,
				ESH.Resources.DataDictionary.Announcement);

			result.WithError(errorMessage);
			return result;
		}

		if (entity.ProfileId != profileId)
		{
			result.WithError(ESH.Resources.ResponseErrors.ForbiddenError403);
			return result;
		}

		await UnitOfWork.AnnouncementRepository
			.RemoveAsync(entity, cancellationToken);

		await UnitOfWork
			.FieldValueAnnouncementRepository
			.RemoveByAnnouncementIdAsync(entity.Id, cancellationToken);

		var owner = new AttachmentOwner(
			nameof(Domain.Announcement),
			entity.Id, Domain.Base.ServerKeyConstant.Key,
			AnnouncementAttachmentSubjectKeys.AnnouncementImage);

		await AttachmentService.DeleteAllAsync(owner, cancellationToken);

		entity.UpdateDateTime = ESH.Utilities.DateTools.DateTimeNow();

		entity.DeleteReasonId = model.DeleteReasonId;
		entity.DeleteReasonDescription = model.Description;

		await UnitOfWork.SaveAsync(cancellationToken);

		var successMessage = string.Format(
			ESH.Resources.Messages.DeleteMessageSuccess,
			ESH.Resources.DataDictionary.Announcement);

		result.WithSuccess(successMessage);

		return result;
	}

	#endregion /DeleteAsync(string id, string deleteReasonId, string profileId)

	#region MapSection

	#region GetClustersAsync(AnnouncementParameters parameters)

	public async Task<Result<List<MapCluster>>> GetClustersAsync(
		AnnouncementParameters parameters,
		CancellationToken cancellationToken = default)
	{
		var result = new Result<List<MapCluster>>();

		var value =
			await UnitOfWork.AnnouncementRepository
				.GetClustersAsync(parameters, cancellationToken);

		if (result.IsSuccess == true)
		{
			result.WithValue(value);
		}

		return result;
	}

	#endregion /GetClustersAsync(AnnouncementParameters parameters)

	#endregion /MapSection

	#region BuildMiniModelsAsync (Helper Method)

	private async Task BuildMiniModelsAsync(
		List<Announcement?> entities,
		List<string> announcementIds,
		List<AnnouncementMiniResponseViewModel> listModels,
		CancellationToken cancellationToken = default)
	{
		var allCategoryIds = entities
			.Select(e => e!.CategoryId)
			.ToList();

		var codeLanguage = ESH.Helpers.CurrentLanguage.Code();

		var searchModel =
			new SearchBySubSystemNameAndRelationIdsAndPropertyNameAndLanguageCodeModel(
			nameof(Domain.Category),
			allCategoryIds,
			Domain.Category.PropertyNameKey,
			codeLanguage
		);

		var localizers = await LanguageLocalizerManager
			.FindAsync(searchModel, cancellationToken);

		var statusIds = entities
			.Select(e => e!.StatusId)
			.ToList();

		var searchModelTitleStatus = new SearchBySubSystemNameAndRelationIdsAndPropertyNameAndLanguageCodeModel(
			nameof(Domain.Status),
			statusIds,
			Domain.Status.TitleProperty,
			codeLanguage
		);

		var profileId = JwtTokenValidator.GetUserId();

		var listIdsAnnouncementHasLiked = new List<string>();

		if (profileId is not null)
		{
			listIdsAnnouncementHasLiked = await UnitOfWork.FavoriteRepository
				.CheckAnnouncementIdsAsync(
					announcementIds, profileId: profileId, cancellationToken);
		}

		foreach (var announcement in entities)
		{
			var categoryDisplayName = localizers
				.Where(current => current.RelationId == announcement!.CategoryId)
				.FirstOrDefault()?.Value ?? string.Empty;

			var price = announcement!
				.FieldValueAnnouncements
				.Where(fv => fv.Field!.FieldType!.Code == FieldTypes.Price)
				.Select(fv => fv.Value)
				.FirstOrDefault();

			var model = new AnnouncementMiniResponseViewModel
			{
				Id = announcement.Id,
				CreateDateTime = announcement.CreateDateTime,
				UpdateDateTime = announcement.UpdateDateTime,

				CategoryId = announcement.CategoryId,
				CategoryTypeId = announcement.Category!.CategoryTypeId,
				CategoryTypeCode = announcement.Category!.CategoryType!.Code,
				CategoryDisplayName = categoryDisplayName,

				Price = price,
				
				UserPhoneNumber =
					string.IsNullOrEmpty(profileId) == false ?
					announcement.Profile!.FullPhoneNumber : null,

				StatusId = announcement.StatusId,
				StatusCode = announcement.Status!.Code,

				DictionaryCheckerId = announcement.DictionaryCheckerId,
				DictionaryCheckerText = announcement.DictionaryChecker?.Text,

				DeleteReasonText = null,
				DeleteReasonId = announcement.DeleteReasonId,
				DeleteReasonDescription = announcement.DeleteReasonDescription,

				IsHidden = announcement.IsHidden,

				Latitude = announcement.Latitude,
				Longitude = announcement.Longitude,

				ButtonFeature =
					StatusButtonFeatureByCode.Get(announcement.Status!.Code)
			};

			switch (announcement.Category.CategoryType.Code)
			{
				case CategoryTypes.Plate:
				{
					var plateNumber = announcement
						.FieldValueAnnouncements
						.Where(fv => fv.Field!.FieldType!.Code == FieldTypes.PlateNumberPart)
						.Select(fv => fv.Value)
						.FirstOrDefault();

					if (plateNumber is null)
					{
						var logError =
							$"[AnnouncementService -> BuildMiniModelsAsync] Plate number not found for announcement {announcement.Id}. | plateNumber is null after fetching from FieldValueAnnouncements with FieldTypeCode {FieldTypes.PlateNumberPart}";

						await LogServerManager.CreateAsync(logError);

						continue;
					}

					model.PlateNumber = plateNumber;

					var plateLetterId = announcement
						.FieldValueAnnouncements
						.Where(fv => fv.Field!.FieldType!.Code == FieldTypes.PlateLetter)
						.Select(fv => fv.Value)
						.FirstOrDefault();

					if (plateLetterId is null)
					{
						var logError =
							$"[AnnouncementService -> BuildMiniModelsAsync] Plate letter not found for announcement {announcement.Id}. | plateLetterId is null after fetching from FieldValueAnnouncements with FieldTypeCode {FieldTypes.PlateLetter}";

						await LogServerManager.CreateAsync(logError);

						continue;
					}

					var plateCode = await UnitOfWork
						.PlateCodeRepository.FindAsync(plateLetterId, cancellationToken:cancellationToken);

					if (plateCode is null)
					{
						var logError =
							$"[AnnouncementService -> BuildMiniModelsAsync] Plate code not found for announcement {announcement.Id}. | plateCode is null after fetching from FieldValueAnnouncements with FieldTypeCode {FieldTypes.PlateLetter} and Id {plateLetterId}";

						await LogServerManager.CreateAsync(logError);

						continue;
					}
					
					model.BlurPlateLetters = announcement.BlurPlateLetters;
					
					if (announcement.BlurPlateLetters.HasValue == true
					    && announcement.BlurPlateLetters.Value == true
					    && announcement.ProfileId != profileId)
					{
						model.PlateLetterArOM = string.Empty.PadLeft(plateCode.ArOm.Length, '#');
						model.PlateLetterEnUS = string.Empty.PadLeft(plateCode.EnUs.Length, '#');
					}
					else
					{
						model.PlateLetterArOM = plateCode.ArOm;
						model.PlateLetterEnUS = plateCode.EnUs;
					}

					var plateStatusId = announcement
						.FieldValueAnnouncements
						.Where(fv => fv.Field!.FieldType!.Code == FieldTypes.PlateStatus)
						.Select(fv => fv.Value)
						.FirstOrDefault();

					if (plateStatusId is null)
					{
						var logError =
							$"[AnnouncementService -> BuildMiniModelsAsync] Plate status not found for announcement {announcement.Id}. | plateStatusId is null after fetching from FieldValueAnnouncements with FieldTypeCode {FieldTypes.PlateStatus}";

						await LogServerManager.CreateAsync(logError);

						continue;
					}

					var plateStatus = await UnitOfWork
						.PlateStatusRepository.FindAsync(plateStatusId, cancellationToken:cancellationToken);

					if (plateStatus is null)
					{
						var logError =
							$"[AnnouncementService -> BuildMiniModelsAsync] Plate status not found for announcement {announcement.Id}. | plateStatus is null after fetching from FieldValueAnnouncements with FieldTypeCode {FieldTypes.PlateStatus} and Id {plateStatusId}";

						var logServer = new LogServer(logError);

						await LogServerManager.CreateAsync(logServer);

						continue;
					}

					model.PlateStatusId = plateStatus.Id;
					model.PlateStatusCode = plateStatus.Code;

					break;
				}
				case CategoryTypes.Phone:
				{
					var phoneNumber = announcement
						.FieldValueAnnouncements
						.Where(fv => fv.Field!.FieldType!.Code == FieldTypes.PhoneBody)
						.Select(fv => fv.Value)
						.FirstOrDefault();

					if (phoneNumber is null)
					{
						var logError =
							$"[AnnouncementService -> BuildMiniModelsAsync] Phone number not found for announcement {announcement.Id}. | phoneNumber is null after fetching from FieldValueAnnouncements with FieldTypeCode {FieldTypes.PhoneBody}";

						var logServer = new LogServer(logError);

						await LogServerManager.CreateAsync(logServer);

						continue;
					}

					model.PhoneNumber = phoneNumber;

					break;
				}
				case CategoryTypes.Property:
				case CategoryTypes.Other:
				{
					var title = announcement
						.FieldValueAnnouncements
						.Where(fv => fv.Field!.FieldType!.Code == FieldTypes.Title)
						.Select(fv => fv.Value)
						.FirstOrDefault();

					model.Title = title;

					break;
				}
			}

			listModels.Add(model);
		}

		listModels.ForEach(m =>
		{
			m.HasLiked = listIdsAnnouncementHasLiked.Contains(m.Id!);
		});

		await LanguageService.LocalizeAsync(
			listModels,
			subSystem: nameof(Domain.Status),
			x => x.StatusId,
			applyValue: (model, text) => model.StatusTitle = text,
			key: Domain.Status.TitleProperty,
			cancellationToken: cancellationToken);

		await LanguageService.LocalizeAsync(
			listModels,
			subSystem: nameof(Domain.DeleteReason),
			x => x.DeleteReasonId,
			applyValue: (model, text) => model.DeleteReasonText = text,
			key: Domain.DeleteReason.TextPropertyLocalizer,
			cancellationToken: cancellationToken);

		await AttachmentService.AttachAsync
			<AnnouncementMiniResponseViewModel, AnnouncementMiniRequestViewModel>(listModels, nameof(Announcement));
	}

	#endregion /BuildMiniModelsAsync

	#region DeserializeFieldConfig

	private IFieldTypeConfig? DeserializeFieldConfig(string fieldTypeCode, string? jsonConfig)
	{
		IFieldTypeConfig? result = null;

		if (string.IsNullOrWhiteSpace(jsonConfig) == true)
		{
			return result;
		}

		var options = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase
		};

		try
		{
			switch (fieldTypeCode)
			{
				case FieldTypes.Int:
				case FieldTypes.Decimal:
					result = JsonSerializer.Deserialize<NumberConfig>(jsonConfig, options);
					break;
				case FieldTypes.String:
					result = JsonSerializer.Deserialize<StringConfig>(jsonConfig, options);
					break;
				case FieldTypes.Title:
					result = JsonSerializer.Deserialize<StringConfig>(jsonConfig, options);
					break;
				case FieldTypes.Text:
					result = JsonSerializer.Deserialize<TextConfig>(jsonConfig, options);
					break;
				case FieldTypes.Description:
					result = JsonSerializer.Deserialize<TextConfig>(jsonConfig, options);
					break;
				case FieldTypes.MultiValue:
					result = JsonSerializer.Deserialize<MultiValueConfig>(jsonConfig, options);
					break;
				case FieldTypes.Attachment:
					result = JsonSerializer.Deserialize<AttachmentConfig>(jsonConfig, options);
					break;
				case FieldTypes.Location:
					result = JsonSerializer.Deserialize<LocationConfig>(jsonConfig, options);
					break;

				case FieldTypes.PlateNumberPart:
					result = JsonSerializer.Deserialize<NumberConfig>(jsonConfig, options);
					break;

				case FieldTypes.PhoneBody:
					result = JsonSerializer.Deserialize<StringConfig>(jsonConfig, options);
					break;

				case FieldTypes.PhoneOperator:
					result = JsonSerializer.Deserialize<MultiValueConfig>(jsonConfig, options);
					break;

				case FieldTypes.PlateStatus:
					result = JsonSerializer.Deserialize<MultiValueConfig>(jsonConfig, options);
					break;
				case FieldTypes.PlateLetter:
					result = JsonSerializer.Deserialize<MultiValueConfig>(jsonConfig, options);
					break;
				case FieldTypes.Region:
					result = JsonSerializer.Deserialize<MultiValueConfig>(jsonConfig, options);
					break;
				case FieldTypes.Price:
					result = JsonSerializer.Deserialize<DecimalConfig>(jsonConfig, options);
					break;
				case FieldTypes.CustomValues:
					result = JsonSerializer.Deserialize<MultiValueConfig>(jsonConfig, options);
					break;

				default:
					throw new NotImplementedException(
						$"Field config deserialization for type '{fieldTypeCode}' is not implemented.");
			}
		}
		catch
		{
			result = null;
		}

		return result;
	}

	#endregion /DeserializeFieldConfig
}

public class ValidatedFieldValue : object
{
	public ValidatedFieldValue() : base()
	{
		Field = null;

		Value = string.Empty;
		FieldId = string.Empty;
	}

	public string Value { get; set; }
	public string FieldId { get; set; }
	public Domain.Field? Field { get; set; }
}