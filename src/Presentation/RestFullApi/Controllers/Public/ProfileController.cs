using Resources;
using AutoMapper;
using Persistence;
using FluentResults;
using ESH.Utilities;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using ESH.ViewModels.Announcement;
using ESH.Constant.Attachment.Announcement;
using ESH.BuildingBlocks.Logging.Contracts;
using ESH.BuildingBlocks.Attachments.Contract;
using ESH.BuildingBlocks.Application.Abstraction;
using ESH.BuildingBlocks.Attachments.Abstraction;
using ESH.BuildingBlocks.Localization.Abstraction;

namespace RestFullApi.Controllers.Public;

/// <summary>
/// مدیریت پروفایل ها
/// </summary>
public class ProfileController : BaseControllerApi
{
	#region Constructor

	public IMapper Mapper { get; }
	public HttpClient HttpClient { get; }
	public IConfiguration Configuration { get; }
	public IHttpContextAccessor HttpContextAccessor { get; }
	public IUnitOfWork UnitOfWork { get; }
	public ILogDetailManager LogDetailManager { get; }
	public ILogServerManager LogServerManager { get; }
	public ILanguageCodeManager LanguageCodeManager { get; }
	private IAttachmentService AttachmentService { get; }
	public IJwtTokenValidator JwtTokenValidator { get; }

	public ProfileController(IMapper mapper, HttpClient httpClient, IConfiguration configuration,
		IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, ILogDetailManager logDetailManager,
		ILogServerManager logServerManager, ILanguageCodeManager languageCodeManager,
		IAttachmentService attachmentService, IJwtTokenValidator jwtTokenValidator) : base()
	{
		Mapper = mapper;
		HttpClient = httpClient;
		Configuration = configuration;
		HttpContextAccessor = httpContextAccessor;
		UnitOfWork = unitOfWork;
		LogDetailManager = logDetailManager;
		LogServerManager = logServerManager;
		LanguageCodeManager = languageCodeManager;
		AttachmentService = attachmentService;
		JwtTokenValidator = jwtTokenValidator;
	}

	#endregion /Constructor

	#region App & Web

	#region [HttpGet(template: "mini")]

	/// <summary>
	/// دریافت دیتای کوچک از کاربر
	/// </summary>
	/// <returns></returns>
	[HttpGet(template: "mini")]
	public async Task<IActionResult> GetMiniProfileAsync()
	{
		var result = new Result<MiniProfileResponseViewModel>();

		var resultToken = JwtTokenValidator.GetUserId();
		
		if (string.IsNullOrEmpty(resultToken) == true)
		{
			result.WithError(ResponseErrors.UnauthorizedError401);
			return ToSampleResult(result);
		}

		var entity =
			await UnitOfWork
				.ProfileRepository
				.FindAsync(resultToken);

		if (entity is null)
		{
			result.WithError(ResponseErrors.ForbiddenError403);
			return ToSampleResult(result);
		}

		if (result.IsSuccess == true)
		{
			var profileResponse = Mapper.Map<MiniProfileResponseViewModel>(entity);

			await AttachmentService.AttachAsync
				<MiniProfileResponseViewModel, MiniProfileRequestViewModel>(profileResponse, nameof(Profile));

			result.WithValue(profileResponse);
		}

		return ToSampleResult(result);
	}

	#endregion /[HttpGet(template: "mini")]

	#region [HttpGet(template: "mini-other/{profileId}")]

	/// <summary>
	/// دریافت مینی پروفایل بقیه افزاد
	/// </summary>
	/// <returns></returns>
	[HttpGet(template: "mini-other/{profileId}")]
	public async Task<IActionResult> GetMiniOtherAsync(string profileId)
	{
		var result = new Result<MiniProfileResponseViewModel>();

		if (string.IsNullOrEmpty(profileId) == true)
		{
			result.WithError(ResponseErrors.RequestNotValid400);
			return ToSampleResult(result);
		}

		var entity =
			await UnitOfWork
				.ProfileRepository
				.FindAsync(profileId);

		if (entity is null || entity.ShowProfileInAnnouncement == false)
		{
			result.WithError(ResponseErrors.ForbiddenError403);
			return ToSampleResult(result);
		}

		if (result.IsSuccess == true)
		{
			var profileResponse = Mapper.Map<MiniProfileResponseViewModel>(entity);

			await AttachmentService.AttachAsync
				<MiniProfileResponseViewModel, MiniProfileRequestViewModel>(profileResponse, nameof(Profile));

			result.WithValue(profileResponse);
		}

		return ToSampleResult(result);
	}

	#endregion /[HttpGet(template: "mini-other/{profileId}")]

	#region [HttpPut(template: "update-profile")]

	/// <summary>
	/// آپدیت پروفایل کاربر
	/// </summary>
	/// <param name="model"></param>
	/// <returns>مدل مینی پروفایل با تغییرات نهایی برای شما برگردانده میشود</returns>
	[HttpPut(template: "update-profile")]
	public async Task<IActionResult>
		UpdateProfileAsync([FromForm] ProfileRequestViewModel model)
	{
		var result =
			new Result<MiniProfileResponseViewModel>();

		var resultToken = JwtTokenValidator.GetUserId();
		
		if (string.IsNullOrEmpty(resultToken) == true)
		{
			result.WithError(ResponseErrors.UnauthorizedError401);
			return ToSampleResult(result);
		}

		var validateModel = model.Validate();

		if (validateModel.IsSuccess == false)
		{
			result.WithErrors(validateModel.Errors);
			return ToSampleResult(result);
		}

		var entity =
			HttpContext.Items[nameof(Domain.Profile)] as Domain.Profile;

		if (entity is null)
		{
			throw new InvalidOperationException(ResponseErrors.ForbiddenError403);
		}

		if (model.FileUpload is not null)
		{
			var owner = new AttachmentOwner(
				SubSystemName: nameof(Profile),
				RelationId: entity.Id,
				ServerId: Domain.Base.ServerKeyConstant.Key,
				SubjectCode: AnnouncementAttachmentSubjectKeys.ProfileImageSmall
			);

			var attachmentResult =
				await AttachmentService
					.UploadAsync(model.FileUpload, owner, AttachmentReplacePolicy.ReplacePrimary);

			if (attachmentResult.IsSuccess == false)
			{
				result.WithErrors(attachmentResult.Errors);
			}
		}

		if (model.FileCover is not null)
		{
			var owner = new AttachmentOwner(
				SubSystemName: nameof(Profile),
				RelationId: entity.Id,
				ServerId: Domain.Base.ServerKeyConstant.Key,
				SubjectCode: AnnouncementAttachmentSubjectKeys.ProfileImageLarge
			);

			var attachmentResult =
				await AttachmentService
					.UploadAsync(model.FileCover, owner, AttachmentReplacePolicy.ReplacePrimary);

			if (attachmentResult.IsSuccess == false)
			{
				result.WithErrors(attachmentResult.Errors);
			}
		}

		if (result.IsSuccess == true)
		{
			entity.DisplayName = model.DisplayName;

			await UnitOfWork.SaveAsync();

			var profileResponse = Mapper.Map<MiniProfileResponseViewModel>(entity);

			await AttachmentService.AttachAsync
				<MiniProfileResponseViewModel, MiniProfileRequestViewModel>(profileResponse, nameof(Profile));

			result.WithValue(profileResponse);

			var successMessage = string.Format
				(Messages.UpdateMessageSuccess, DataDictionary.Profile);

			result.WithSuccess(successMessage);
		}

		return ToSampleResult(result);
	}

	#endregion /[HttpPut(template: "update-profile")]

	#region [HttpPut(template: "update-language")]

	/// <summary>
	/// تغییر زبان
	/// </summary>
	/// <returns></returns>
	[HttpPut(template: "update-language/{languageCode}")]
	public async Task<IActionResult> UpdateLanguageAsync(string languageCode)
	{
		var result =
			new Result<MiniProfileResponseViewModel>();

		var resultToken = JwtTokenValidator.GetUserId();
		
		if (string.IsNullOrEmpty(resultToken) == true)
		{
			result.WithError(ResponseErrors.UnauthorizedError401);
			return ToSampleResult(result);
		}

		var entity =
			HttpContext.Items[nameof(Domain.Profile)] as Domain.Profile;

		if (entity is null)
		{
			throw new InvalidOperationException(ResponseErrors.ForbiddenError403);
		}

		var languageCodeEntity =
			await LanguageCodeManager.FindLanguageByCodeAsync(languageCode);

		if (languageCodeEntity is null)
		{
			result.WithError(ESH.Helpers.ResponseHelper.Response400WithCode(10));
			return ToSampleResult(result);
		}

		if (result.IsSuccess == true)
		{
			entity.LanguageCodeId = languageCodeEntity.Id;
			await UnitOfWork.SaveAsync();

			var profileResponse = Mapper.Map<MiniProfileResponseViewModel>(entity);

			await AttachmentService.AttachAsync
				<MiniProfileResponseViewModel, MiniProfileRequestViewModel>
				(profileResponse, nameof(Profile));

			result.WithValue(profileResponse);
		}

		return ToSampleResult(result);
	}

	#endregion /[HttpPut(template: "update-profile")]

	#region [HttpDelete(template: "delete-all-image-profile")]

	/// <summary>
	/// حذف تصویر پروفایل
	/// </summary>
	/// <returns>مدل مینی پروفایل با تغییرات نهایی برای شما برگردانده میشود</returns>
	[HttpDelete(template: "delete-all-image-profile")]
	public async Task<IActionResult> DeleteAllImageProfileAsync()
	{
		var result = new Result<MiniProfileResponseViewModel>();

		var resultToken = JwtTokenValidator.GetUserId();
		
		if (string.IsNullOrEmpty(resultToken) == true)
		{
			result.WithError(ResponseErrors.UnauthorizedError401);
			return ToSampleResult(result);
		}

		var entity =
			HttpContext.Items[nameof(Domain.Profile)] as Domain.Profile;

		if (entity is null)
		{
			result.WithError(ResponseErrors.ForbiddenError403);
			return ToSampleResult(result);
		}

		var owner = new AttachmentOwner(
			SubSystemName: nameof(Profile),
			RelationId: entity.Id,
			ServerId: Domain.Base.ServerKeyConstant.Key,
			SubjectCode: AnnouncementAttachmentSubjectKeys.ProfileImageSmall
		);

		var deleteImageResult = await AttachmentService.DeleteAllAsync(owner);

		if (deleteImageResult.IsSuccess == true)
		{
			await UnitOfWork.SaveAsync();

			var profileResponse = Mapper.Map<MiniProfileResponseViewModel>(entity);

			await AttachmentService.AttachAsync
				<MiniProfileResponseViewModel, MiniProfileRequestViewModel>(profileResponse, nameof(Profile));

			result.WithValue(profileResponse);
		}

		return ToSampleResult(result);
	}

	#endregion /[HttpDelete(template: "delete-all-image-profile")]

	#region [HttpDelete(template: "delete-image-profile")]

	/// <summary>
	/// حذف تصویر پروفایل
	/// </summary>
	/// <returns>مدل مینی پروفایل با تغییرات نهایی برای شما برگردانده میشود</returns>
	[HttpDelete(template: "delete-image-profile")]
	public async Task<IActionResult> DeleteImageProfileAsync(string attachmentId)
	{
		var result = new Result<MiniProfileResponseViewModel>();

		var resultToken = JwtTokenValidator.GetUserId();
		
		if (string.IsNullOrEmpty(resultToken) == true)
		{
			result.WithError(ResponseErrors.UnauthorizedError401);
			return ToSampleResult(result);
		}

		var entity =
			HttpContext.Items[nameof(Domain.Profile)] as Domain.Profile;

		if (entity is null)
		{
			result.WithError(ResponseErrors.ForbiddenError403);
			return ToSampleResult(result);
		}

		var owner = new AttachmentOwner(
			SubSystemName: nameof(Profile),
			RelationId: entity.Id,
			ServerId: Domain.Base.ServerKeyConstant.Key,
			SubjectCode: AnnouncementAttachmentSubjectKeys.ProfileImageSmall
		);

		var deleteImageResult =
			await AttachmentService.DeleteByIdAsync(owner, attachmentId);

		if (deleteImageResult.IsSuccess == false)
		{
			result.WithErrors(deleteImageResult.Errors);
		}

		if (result.IsSuccess == true)
		{
			await UnitOfWork.SaveAsync();

			var profileResponse = Mapper.Map<MiniProfileResponseViewModel>(entity);

			await AttachmentService.AttachAsync
				<MiniProfileResponseViewModel, MiniProfileRequestViewModel>(profileResponse, nameof(Profile));

			result.WithValue(profileResponse);
		}

		return ToSampleResult(result);
	}

	#endregion /[HttpDelete(template: "delete-image-profile")]

	#region [HttpPost("get-by-ids")]

	/// <summary>
	/// دریافت لیستی از پروفایل ها
	/// </summary>
	/// <param name="ids">شناسه پروفایل های مورد نظر</param>
	/// <returns></returns>
	[HttpPost("get-by-ids")]
	public async Task<IActionResult> GetByIdsAsync([FromBody] List<string> ids)
	{
		var result = new Result<List<UiSelectModel>>();

		var hasEmpty = ids.Any(x => string.IsNullOrEmpty(x));

		if (hasEmpty == true)
		{
			result.WithError(ResponseErrors.RequestNotValid400);
		}

		if (result.IsSuccess == true)
		{
			List<UiSelectModel> value =
				await UnitOfWork.ProfileRepository.FindByIdsAsync(ids);

			result.WithValue(value);
		}

		return ToSampleResult(result);
	}

	#endregion /[HttpPost("get-by-ids")]
	
	#endregion /App & Web
}