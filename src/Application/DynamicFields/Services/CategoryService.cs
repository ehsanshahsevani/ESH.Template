using ESH.Resources;
using AutoMapper;
using Domain.Base;
using Persistence;
using FluentResults;
using Domain.Constants;
using ESH.ViewModels.Shared;
using ESH.ViewModels.Announcement;
using DynamicFields.Abstraction;
using Microsoft.AspNetCore.Http;
using ESH.Constant.Attachment.Announcement;


using ESH.BuildingBlocks.Attachments.Abstraction;
using ESH.BuildingBlocks.Attachments.Contract;
using ESH.BuildingBlocks.Localization.Contract;
using ESH.ViewModels.Announcement.ModelParameters;
using ESH.BuildingBlocks.Localization.Abstraction;
using ESH.BuildingBlocks.RequestFeatures;
using ESH.SeedworkSystem.ViewModel.Localizer;
using ESH.Utilities;

namespace DynamicFields.Services;

public class CategoryService : object, ICategoryService
{
	#region Constructor

	public CategoryService(
		IUnitOfWork unitOfWork,
		IAttachmentService attachmentService,
		ILanguageService languageService, IMapper mapper,
		ILanguageLocalizerManager languageLocalizerManager,
		IAnnouncementService announcementService)
	{
		Mapper = mapper;
		UnitOfWork = unitOfWork;
		LanguageService = languageService;
		AttachmentService = attachmentService;
		AnnouncementService = announcementService;
		LanguageLocalizerManager = languageLocalizerManager;
	}

	private IMapper Mapper { get; }
	private IUnitOfWork UnitOfWork { get; }
	private ILanguageService LanguageService { get; }
	private IAttachmentService AttachmentService { get; }
	private ILanguageLocalizerManager LanguageLocalizerManager { get; }
	public IAnnouncementService AnnouncementService { get; }

	#endregion /Constructor

	#region GetAsync()

	public async Task<Result<List<CategoryResponseViewModel>>> GetAsync(CancellationToken cancellationToken = default)
	{
		var result =
			new Result<List<CategoryResponseViewModel>>();

		var entities =
			await UnitOfWork.CategoryRepository.GetAllAsync(cancellationToken);

		if (result.IsSuccess == true)
		{
			var value =
				Mapper.Map<List<CategoryResponseViewModel>>(entities);

			await LanguageService.LocalizeAsync(
				value,
				subSystem: nameof(Domain.Category),
				x => x.ParentId,
				applyValue: (vm, text) => vm.ParentDisplayName = text,
				key: Domain.Category.PropertyNameKey, cancellationToken: cancellationToken);

			await LanguageService.LocalizeAsync(
				value,
				subSystem: nameof(Domain.Category),
				x => x.Id,
				applyValue: (vm, text) => vm.Name = text,
				key: Domain.Category.PropertyNameKey, cancellationToken: cancellationToken);

			await AttachmentService.AttachAsync<
				CategoryResponseViewModel, CategoryRequestViewModel>(value, nameof(Domain.Category));

			result.WithValue(value);
		}

		return result;
	}

	#endregion /GetAsync()

	#region GetByIdAsync(string id)

	public async Task<Result<CategoryRequestViewModel>>
		GetByIdAsync(string id, CancellationToken cancellationToken = default)
	{
		var result = new Result<CategoryRequestViewModel>();

		var entity = await UnitOfWork
			.CategoryRepository.FindAsync(id, cancellationToken: cancellationToken);

		if (entity is null)
		{
			var errorMessage = string.Format(
					Messages.NotFoundError,
					DataDictionary.Category);

			result.WithError(errorMessage);
		}

		if (result.IsSuccess == true)
		{
			var value = Mapper.Map<CategoryRequestViewModel>(entity);

			result.WithValue(value);
		}

		return result;
	}

	#endregion /GetByIdAsync(string id)

	#region SearchByTextAsync(string text)

	public async Task<Result<List<CategoryResponseViewModel>>>
		SearchByTextAsync(string text, CancellationToken cancellationToken = default)
	{
		var result =
			new Result<List<CategoryResponseViewModel>>();

		var categoryIds =
			await LanguageLocalizerManager.SearchByTextAsync(
				text, nameof(Domain.Category), Domain.Category.PropertyNameKey, cancellationToken);

		var entities =
			await UnitOfWork.CategoryRepository
				.GetAllWithAnnouncementCheckAsync(categoryIds, cancellationToken);

		if (result.IsSuccess == true)
		{
			var value =
				Mapper.Map<List<CategoryResponseViewModel>>(entities);

			await LanguageService.LocalizeAsync(
				value,
				subSystem: nameof(Domain.Category),
				x => x.Id,
				applyValue: (vm, name) => vm.Name = name,
				key: Domain.Category.PropertyNameKey, cancellationToken: cancellationToken);

			await AttachmentService
				.AttachAsync<CategoryResponseViewModel
					, CategoryRequestViewModel>(value, nameof(Domain.Category));

			result.WithValue(value);
		}

		return result;
	}

	#endregion /SearchByTextAsync(string text)

	#region GetAllPinInHomeAsync(bool? isActive = true, bool withAnnouncement = true)

	public async Task<Result<List<CategoryResponseViewModel>>> GetAllPinInHomeAsync(
		bool? isActive = true,
		bool withAnnouncement = true,
		CancellationToken cancellationToken = default)
	{
		var result =
			new Result<List<CategoryResponseViewModel>>();

		var entities =
			await UnitOfWork.CategoryRepository.GetAllPinInHomeAsync(
				isActive: isActive,
				takeAnnouncement: 10,
				cancellationToken: cancellationToken);

		var announcementsResult =
			new Result<List<AnnouncementMiniResponseViewModel>>();
		
		if (withAnnouncement == true && entities.Any() == true)
		{
			var ids = 
				entities.Select(x => x.Id).ToList();

			var announcementIds =
				await UnitOfWork
					.AnnouncementRepository
					.GetIdsByCategoryIdsAsync(
						ids, takeAnnouncement: 10, cancellationToken);

			var announcementParameters =
				new AnnouncementParameters()
				{
					Ids = announcementIds,
					IsActive = true,
					IsHidden = false,
					IsDeleted = false,
			
					PhoneNumber = null,
			
					MapRequest = null,

					// ignore in list
					// PageSize = 200,
					// PageNumber = 1,
				};
			
			var statusCode30 = await UnitOfWork
				.StatusRepository.FindByCodeAsync(30, cancellationToken);
			
			if (statusCode30 is null)
			{
				throw new NullReferenceException(nameof(statusCode30));
			}
			
			announcementParameters.StatusId = statusCode30.Id;
	
			announcementsResult = await AnnouncementService
				.GetAllInListAsync(announcementParameters, cancellationToken);
		}

		if (result.IsSuccess == true)
		{
			var value =
				Mapper.Map<List<CategoryResponseViewModel>>(entities);

		if (withAnnouncement == true && entities.Any() == true)
			{
				foreach (var item in value)
				{
					item.Announcements = announcementsResult.Value
						.Where(current => current.CategoryId == item.Id)
						.ToList();
				}
			}

			await LanguageService.LocalizeAsync(
				value,
				subSystem: nameof(Domain.Category),
				x => x.Id,
				applyValue: (vm, name) => vm.Name = name,
				key: Domain.Category.PropertyNameKey, cancellationToken: cancellationToken);

			await AttachmentService
				.AttachAsync<CategoryResponseViewModel
					, CategoryRequestViewModel>(value, nameof(Domain.Category));

			result.WithValue(value);
		}

		return result;
	}

	#endregion GetAllPinInHomeAsync(bool? isActive = true

	#region GetParentsAsync()

	public async Task<Result<List<CategoryResponseViewModel>>> GetParentsAsync(CancellationToken cancellationToken = default)
	{
		var result = new Result<List<CategoryResponseViewModel>>();

		var entities =
			await UnitOfWork.CategoryRepository.GetParentCategoriesAsync(cancellationToken);

		if (result.IsSuccess == true)
		{
			var value =
				Mapper.Map<List<CategoryResponseViewModel>>(entities);

			await LanguageService.LocalizeAsync(
				value,
				subSystem: nameof(Domain.Category),
				x => x.Id,
				applyValue: (vm, text) => vm.Name = text,
				key: Domain.Category.PropertyNameKey, cancellationToken: cancellationToken);

			await AttachmentService.AttachAsync<
				CategoryResponseViewModel, CategoryRequestViewModel>(value, nameof(Domain.Category));

			result.WithValue(value);
		}

		return result;
	}

	#endregion /GetParentsAsync()

	#region GetChildrenAsync(string parentId)

	public async Task<Result<List<CategoryResponseViewModel>>>
		GetChildrenAsync(string parentId, CancellationToken cancellationToken = default)
	{
		var result = new Result<List<CategoryResponseViewModel>>();

		var entities =
			await UnitOfWork.CategoryRepository
				.GetChildrenCategoriesAsync(parentId, cancellationToken);

		if (result.IsSuccess == true)
		{
			var value =
				Mapper.Map<List<CategoryResponseViewModel>>(entities);

			await LanguageService.LocalizeAsync(
				value,
				subSystem: nameof(Domain.Category),
				x => x.Id,
				applyValue: (vm, text) => vm.Name = text,
				key: Domain.Category.PropertyNameKey, cancellationToken: cancellationToken);

			await AttachmentService.AttachAsync<
				CategoryResponseViewModel, CategoryRequestViewModel>(value, nameof(Domain.Category));

			result.WithValue(value);
		}

		return result;
	}

	#endregion /GetChildrenAsync(string parentId)

	#region GetAllWithPageAsync(CategoryParameters parameters)

	public async Task<Result<PagedListResult<CategoryResponseViewModel>>>
		GetAllWithPageAsync(CategoryParameters parameters, CancellationToken cancellationToken = default)
	{
		var result = new Result<PagedListResult<CategoryResponseViewModel>>();

		var entities =
			await UnitOfWork.CategoryRepository
				.GetAllInPageForAdminAsync(parameters, cancellationToken);

		if (result.IsSuccess == true)
		{
			var value =
				Mapper.Map<PagedList<CategoryResponseViewModel>>(entities);

			var valuePack = new PagedListResult<CategoryResponseViewModel>(value, entities.MetaData);

			await LanguageService.LocalizeAsync(
				value,
				subSystem: nameof(Domain.Category),
				x => x.ParentId,
				applyValue: (vm, text) => vm.ParentDisplayName = text,
				key: Domain.Category.PropertyNameKey, cancellationToken: cancellationToken);

			await LanguageService.LocalizeAsync(
				value,
				subSystem: nameof(Domain.Category),
				x => x.Id,
				applyValue: (vm, text) => vm.Name = text,
				key: Domain.Category.PropertyNameKey, cancellationToken: cancellationToken);

			await AttachmentService.AttachAsync<
				CategoryResponseViewModel, CategoryRequestViewModel>(value, nameof(Domain.Category));

			result.WithValue(value: valuePack);
		}

		return result;
	}

	#endregion /GetAllWithPageAsync(CategoryParameters parameters)

	#region GetDropDownDataAsync()

	public async Task<Result<List<UiSelectModel>>> GetDropDownDataAsync(CancellationToken cancellationToken = default)
	{
		var result = new Result<List<UiSelectModel>>();

		List<UiSelectModel> value =
			await UnitOfWork.CategoryRepository.GetSelectValues(cancellationToken);

		await LanguageService.LocalizeAsync(
			value,
			subSystem: nameof(Domain.Category),
			x => x.ValueId,
			applyValue: (vm, text) => vm.Value = text,
			key: Domain.Category.PropertyNameKey, cancellationToken: cancellationToken);

		if (result.IsSuccess == true)
		{
			result.WithValue(value);
		}

		return result;
	}

	#endregion /GetDropDownDataAsync()

	#region CreateAsync(CategoryRequestViewModel model)

	public async Task<Result> CreateAsync(
		CategoryRequestViewModel model,
		IFormFile fileLarge,
		IFormFile fileSmall,
		CancellationToken cancellationToken = default)
	{
		var result = new Result();

		var entity = Mapper.Map<Domain.Category>(model);

		entity.Code = CategoryTypes.Other;

		if (model.ParentId is not null)
		{
			var parent =
				await UnitOfWork.CategoryRepository
					.FindAsync(model.ParentId, cancellationToken: cancellationToken);

			if (parent is null)
			{
				var errorMessage = string.Format(
					Messages.NotFoundError,
					DataDictionary.Category);

				result.WithError(errorMessage);
			}
			else if (parent.CategoryType!.Code == CategoryTypes.Property)
			{
				entity.Code = CategoryTypes.Property;
			}
		}

		var categoryType = await UnitOfWork
			.CategoryTypeRepository.FindByCodeAsync(
				entity.Code, cancellationToken);

		if (categoryType is null)
		{
			var errorMessage = string.Format(
				Messages.NotFoundError,
				DataDictionary.CategoryType);

			result.WithError(errorMessage);
		}

		entity.CategoryTypeId = categoryType!.Id;

		if (result.IsSuccess == true)
		{
			var ownerLarge = new AttachmentOwner(
				nameof(Domain.Category), entity.Id,
				ServerId: ServerKeyConstant.Key,
				AnnouncementAttachmentSubjectKeys.CategoryImageLarge);

			var resultAttachmentLarge = await AttachmentService
				.UploadAsync(fileLarge, ownerLarge, AttachmentReplacePolicy.ReplaceAll, cancellationToken);

			result.WithErrors(resultAttachmentLarge.Errors);

			var ownerSmall = new AttachmentOwner(
				nameof(Domain.Category), entity.Id,
				ServerId: ServerKeyConstant.Key,
				AnnouncementAttachmentSubjectKeys.CategoryImageSmall);

			var resultAttachmentSmall = await AttachmentService
				.UploadAsync(fileSmall, ownerSmall, AttachmentReplacePolicy.ReplaceAll, cancellationToken);

			result.WithErrors(resultAttachmentSmall.Errors);

			// ---
			var owner = new LocalizationOwner(
				nameof(Domain.Category),
				entity.Id, Domain.Category.PropertyNameKey);

			var valueLocalizers =
				Mapper.Map<List<ValueLocalizer>>(model.Name);

			var resultLanguageServer =
				await LanguageService.AddAsync(owner, valueLocalizers, cancellationToken);

			if (resultLanguageServer.IsSuccess == true)
			{
				foreach (var valueLocalizer in valueLocalizers)
				{
					entity.Description += $"{valueLocalizer.Value} ";
				}

				entity.Description = entity.Description!.Trim();
			}

			result.WithErrors(resultLanguageServer.Errors);
		}

		if (result.IsSuccess == true)
		{
			await UnitOfWork.CategoryRepository.AddAsync(entity, cancellationToken);

			await UnitOfWork.SaveAsync(cancellationToken);

			var successMessage = string.Format(
				Messages.CreateSuccessMessage,
				DataDictionary.Category);

			result.WithSuccess(successMessage);
		}

		return result;
	}

	#endregion CreateAsync(CategoryRequestViewModel model)

	#region UpdateAsync(CategoryRequestViewModel model)

	public async Task<Result> UpdateAsync(
		CategoryRequestViewModel model,
		CancellationToken cancellationToken = default)
	{
		var result = new Result();

		var entity = await UnitOfWork
			.CategoryRepository.FindAsync(model.Id!, isActive: null);

		if (entity is null)
		{
			var errorMessage = string.Format(
				Messages.NotFoundError,
				DataDictionary.Category);

			result.WithError(errorMessage);
			return result;
		}

		if (string.IsNullOrEmpty(model.ParentId) == false)
		{
			var parentEntity = await UnitOfWork
				.CategoryRepository.FindAsync(model.ParentId!, isActive: null);

			if (parentEntity is null)
			{
				var errorMessage = string.Format(
					Messages.NotFoundError,
					DataDictionary.Parent);

				result.WithError(errorMessage);
				return result;
			}
		}

		if (result.IsSuccess == true)
		{
			entity.ParentId = model.ParentId;
			entity.PinInHome = model.PinInHome ?? null;
			entity.Ordering = model.Ordering ?? 100_000;

			await UnitOfWork.SaveAsync(cancellationToken);

			var successMessage = string.Format(
				Messages.UpdateMessageSuccess,
				DataDictionary.Category);

			result.WithSuccess(successMessage);
		}

		return result;
	}

	#endregion UpdateAsync(CategoryRequestViewModel model)

	#region UpdateNameAsync(string id, List<ValueLocalizerViewModel> name)

	public async Task<Result> UpdateNameAsync(
		string id, List<ValueLocalizerViewModel> name, CancellationToken cancellationToken = default)
	{
		var result = new Result();

		if (name.Any() == false)
		{
			result.WithError(ESH.Helpers.ResponseHelper.Response400WithCode(10));
			return result;
		}

		if (string.IsNullOrWhiteSpace(id) == true)
		{
			result.WithError(ESH.Helpers.ResponseHelper.Response400WithCode(20));
			return result;
		}

		var entity =
			await UnitOfWork.CategoryRepository.FindAsync(id, isActive: null, cancellationToken: cancellationToken);

		if (entity is null)
		{
			var errorMessage = string.Format(
				Messages.NotFoundError,
				DataDictionary.Category);

			result.WithError(errorMessage);
			return result;
		}

		var owner =
			new LocalizationOwner
			(
				RelationId: entity.Id,
				SubSystemName: nameof(Domain.Category),
				PropertyName: Domain.Category.PropertyNameKey
			);

		var localizers = Mapper.Map<List<ValueLocalizer>>(name);

		result = await LanguageService
			.UpdateAsync(owner, localizers, cancellationToken);

		if (result.IsSuccess == true)
		{
			if (result.IsSuccess == true)
			{
				foreach (var valueLocalizer in localizers)
				{
					entity.Description += $"{valueLocalizer.Value} ";
				}

				entity.Description = entity.Description!.Trim();
			}

			await UnitOfWork.SaveAsync(cancellationToken);

			var successMessage = string.Format(
				Messages.UpdateMessageSuccess,
				DataDictionary.Category);

			result.WithSuccess(successMessage);
		}

		return result;
	}

	#endregion /UpdateNameAsync(string id, List<ValueLocalizerViewModel> name)

	#region UpdateImageAsync(string attachmentSubjectKey, string id, IFormFile file)

	public async Task<Result> UpdateImageAsync(
		string attachmentSubjectKey,
		string id,
		IFormFile file,
		CancellationToken cancellationToken = default)
	{
		var result = new Result();

		if (string.IsNullOrEmpty(id) == true)
		{
			result.WithError(ESH.Helpers.ResponseHelper.Response400WithCode(10));
			return result;
		}

		var entity =
			await UnitOfWork
				.CategoryRepository.FindAsync(id, isActive: null, cancellationToken: cancellationToken);

		if (entity is null)
		{
			var errorMessage = string.Format(
				Messages.NotFoundError,
				DataDictionary.Category);

			result.WithError(errorMessage);
			return result;
		}

		var owner = new AttachmentOwner(
			SubSystemName: nameof(Domain.Category),
			RelationId: entity.Id,
			ServerId: ServerKeyConstant.Key,
			SubjectCode: attachmentSubjectKey
		);

		var uploadResult =
			await AttachmentService.UploadAsync(
				file: file,
				owner: owner,
				replacePolicy: AttachmentReplacePolicy.ReplaceAll, cancellationToken: cancellationToken);

		result.WithErrors(uploadResult.Errors);

		if (result.IsSuccess == true)
		{
			await UnitOfWork.SaveAsync(cancellationToken);

			var successMessage = string.Format(
				Messages.UpdateMessageSuccess,
				DataDictionary.Category);

			result.WithSuccess(successMessage);
		}

		return result;
	}

	#endregion UpdateImageAsync(string attachmentSubjectKey, string id, IFormFile file)

	#region ChangeActivationAsync(string id)

	public async Task<Result<CategoryResponseViewModel>>
		ChangeActivationAsync(string id, CancellationToken cancellationToken = default)
	{
		var result = new Result<CategoryResponseViewModel>();

		var entity = await UnitOfWork
			.CategoryRepository.FindAdminAsync(id, cancellationToken);

		if (entity == null)
		{
			var errorMessage = string.Format(
				Messages.NotFoundError,
				DataDictionary.Category);

			result.WithError(errorMessage);
		}

		if (result.IsSuccess == true)
		{
			entity!.IsActive = !entity.IsActive;
			entity.UpdateDateTime = DateTools.DateTimeNow();

			var value = Mapper.Map<CategoryResponseViewModel>(entity);

			await UnitOfWork.SaveAsync(cancellationToken);

			var successMessage = string.Format(
				Messages.UpdateMessageSuccess, DataDictionary.Category);

			result.WithSuccess(successMessage);

			result.WithValue(value);
		}

		return result;
	}

	#endregion /ChangeActivationAsync(string id)

	#region DeleteAsync(string id)

	public async Task<Result<string>> DeleteAsync(
		string id,
		CancellationToken cancellationToken = default)
	{
		var result = new Result<string>();

		var entity = await UnitOfWork
			.CategoryRepository.FindAsync(id, isActive: null, cancellationToken: cancellationToken);

		if (entity == null)
		{
			throw new ArgumentNullException(nameof(entity));
		}

		// var hasAnnouncement = await UnitOfWork
		// 	.CategoryRepository.HasAnnouncementAsync(
		// 		categoryId: id, cancellationToken: cancellationToken);

		var hasChild = await UnitOfWork
			.CategoryRepository.HasChildAsync(
				categoryId: id, cancellationToken: cancellationToken);

		// hasAnnouncement == true || 
		if (hasChild == true)
		{
			var errorMessage = string.Format(
				Messages.RelationIsActiveError);

			result.WithError(errorMessage);

			return result;
		}

		if (result.IsSuccess == true)
		{
			await UnitOfWork.CategoryRepository.RemoveAsync(entity, cancellationToken);
			await UnitOfWork.SaveAsync(cancellationToken);

			var successMessage = string.Format(
				Messages.DeleteMessageSuccess,
				DataDictionary.Category);

			result.WithSuccess(successMessage);

			result.WithValue(entity.Id);
		}

		return result;
	}

	#endregion /DeleteAsync(string id)
}