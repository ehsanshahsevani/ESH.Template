using AutoMapper;
using Persistence;
using FluentResults;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using ESH.BuildingBlocks.Attachments.Abstraction;
using ESH.BuildingBlocks.Localization.Abstraction;
using ESH.BuildingBlocks.Logging.Contracts;
using ESH.ViewModels.Announcement;

namespace RestFullApi.Controllers.Admin;

/// <summary>
/// ادمین - مدیریت پروفایل
/// </summary>
public class AdminProfileController : BaseControllerApi
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

	public AdminProfileController(IMapper mapper, HttpClient httpClient, IConfiguration configuration,
		IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, ILogDetailManager logDetailManager,
		ILogServerManager logServerManager, ILanguageCodeManager languageCodeManager,
		IAttachmentService attachmentService) : base()
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
	}

	#endregion /Constructor

	#region AdminPanel

	#region [HttpGet(template: "mini")]

	/// <summary>
	/// دریافت دیتای کوچک از کاربر
	/// </summary>
	/// <returns></returns>
	[HttpGet(template: "mini")]
	public async Task<IActionResult> GetMiniProfileAsync()
	{
		var result = new Result<MiniProfileResponseViewModel>();

		var userId = "x";

		var entity =
			await UnitOfWork
				.ProfileRepository
				.FindAsync(userId);

		if (entity is null)
		{
			result.WithError(ESH.Resources.ResponseErrors.ForbiddenError403);
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
	
	#endregion /AdminPanel

	#region Structure

	#region [HttpPost(template: "awaiting-queue/{userId}/{phoneNumber}")]

	/// <summary>
	/// این بخش برای اتصال به سرور اصلی طراحی شده است
	/// </summary>
	/// <param name="userId">شناسه کاربر </param>
	/// <param name="model"></param>
	/// <returns></returns>
	[HttpPost(template: "awaiting-queue/{userId}")]
	public async Task<IActionResult> AwaitingQueueAsync(string userId, [FromBody] AwaitingQueueModel model)
	{
		var result = new FluentResults.Result();

		Domain.Profile profile = new Domain.Profile(userId, model.FullPhoneNumber);

		if (result.IsSuccess== true)
		{
			await UnitOfWork.ProfileRepository.AddAsync(profile);

			await UnitOfWork.SaveAsync();

			result.WithSuccess(Domain.Base.ServerKeyConstant.Key);
		}
		
		return ToSampleResult(result);
	}

	#endregion /[HttpPost(template: "awaiting-queue/{userId}/{phoneNumber}")]

	#endregion /Structure
}

public class AwaitingQueueModel
{
	public string FullPhoneNumber { get; set; }
}