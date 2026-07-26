using Domain;
using AutoMapper;
using ESH.Helpers;
using Persistence;
using FluentResults;
using Infrastructure;
using ESH.Utilities.Network;
using ESH.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc;
using DynamicFields.Abstraction;
using ESH.Constant.Announcement;
using ESH.ViewModels.Announcement;
using ESH.ViewModels.Announcement.MapApp;
using ESH.BuildingBlocks.Logging.Contracts;
using ESH.BuildingBlocks.Application.Abstraction;
using ESH.BuildingBlocks.Localization.Abstraction;
using ESH.ViewModels.Announcement.ModelParameters;


namespace RestFullApi.Controllers.Public;

/// <summary>
/// مدیریت آگهی
/// </summary>

public class AnnouncementController : BaseControllerApi
{
	#region Constants

	private const string ZoomKeyInConfiguration = "ZoomLevelForSwitch";

	#endregion /Constants

	#region DI & Constructor

	public IMapper Mapper { get; }
	public HttpClient HttpClient { get; }
	public IConfiguration Configuration { get; }
	public IHttpContextAccessor HttpContextAccessor { get; }
	public IUnitOfWork UnitOfWork { get; }
	public ILogDetailManager LogDetailManager { get; }
	public ILogServerManager LogServerManager { get; }
	public ILanguageCodeManager LanguageCodeManager { get; }
	private IJwtTokenValidator JwtTokenValidator { get; }
	private IAnnouncementService AnnouncementService { get; }

	public AnnouncementController(
		IMapper mapper,
		HttpClient httpClient,
		IConfiguration configuration,
		IHttpContextAccessor httpContextAccessor,
		IUnitOfWork unitOfWork, ILogDetailManager logDetailManager,
		ILogServerManager logServerManager,
		ILanguageCodeManager languageCodeManager,
		IAnnouncementService announcementService,
		IJwtTokenValidator jwtTokenValidator)
		: base()
	{
		Mapper = mapper;
		HttpClient = httpClient;
		Configuration = configuration;
		HttpContextAccessor = httpContextAccessor;
		UnitOfWork = unitOfWork;
		LogDetailManager = logDetailManager;
		LogServerManager = logServerManager;
		LanguageCodeManager = languageCodeManager;
		JwtTokenValidator = jwtTokenValidator;
		AnnouncementService = announcementService;
	}

	#endregion /DI & Constructor

	#region [HttpPost(template: "get")]

	/// <summary>
	/// دریافت لیست همه به صورت صفحه بندی شده
	/// </summary>
	/// <param name="parameters"></param>
	/// <returns></returns>
	
	[HttpPost(template: "get")]
	
	public async Task<IActionResult> GetAllWithPageAsync
		([FromBody] AnnouncementParameters parameters)
	{
		parameters.IsHidden = false;

		parameters.IsActive = true;
		parameters.IsDeleted = false;

		parameters.PhoneNumber = null;
		
		var statusCode30 = await UnitOfWork
			.StatusRepository.FindByCodeAsync(AnnouncementStatusCodes.Publish);

		if (statusCode30 is null)
		{
			throw new NullReferenceException(nameof(statusCode30));
		}

		parameters.StatusId = statusCode30.Id;

		var result =
			await AnnouncementService
				.GetAllWithPageAsync(parameters);

		return ToSampleResult(result);
	}

	#endregion /[HttpPost(template: "get")]

	#region [HttpPost(template: "get-my-announcements")]

	/// <summary>
	/// دریافت لیست آگهی های من
	/// </summary>
	/// <param name="parameters"></param>
	/// <returns></returns>
	
	[HttpPost(template: "get-my-announcements")]

	public async Task<IActionResult> GetAllWithPageProfileAsync
		([FromBody] AnnouncementParameters parameters)
	{
		var result = new Result<PagedListResult<AnnouncementMiniResponseViewModel>>();

		parameters.IsActive = true;
		parameters.IsDeleted = false;

		var tokenResult = JwtTokenValidator.GetUserId();
		
		if (string.IsNullOrEmpty(tokenResult) == true)
		{
			result.WithError(Resources.ResponseErrors.UnauthorizedError401);
			
			return ToSampleResult(result);
		}

		var profile =
			HttpContext.Items[key: nameof(Domain.Profile)] as Domain.Profile;

		parameters.ProfileId = profile!.Id;

		result =
			await AnnouncementService
				.GetAllWithPageAsync(parameters);

		return ToSampleResult(result);
	}

	#endregion /[HttpPost(template: "get-my-announcements")]

	#region [HttpPost(template: "get-favorite")]

	/// <summary>
	/// لیست علاقه مندی ها
	/// </summary>
	/// <param name="parameters"></param>
	/// <returns></returns>
	[HttpPost(template: "get-favorite")]
	public async Task<IActionResult> GetAllWithPageFavoriteAsync([FromBody] AnnouncementParameters parameters)
	{
		var result = new Result<PagedListResult<AnnouncementMiniResponseViewModel>>();

		if (JwtTokenValidator.IsAuthenticated() == false)
		{
			result.WithError(Resources.ResponseErrors.UnauthorizedError401);
			return ToSampleResult(result);
		}
		
		parameters.HasLiked = true;
		parameters.IsHidden = null;
		parameters.ProfileFavoriteId = JwtTokenValidator.GetUserId()!;

		parameters.IsActive = true;
		parameters.IsDeleted = false;

		parameters.OrderBy = AnnouncementOrderByFields.FavoriteDateDesc;
		
		var statusCode30 = await UnitOfWork
			.StatusRepository.FindByCodeAsync(AnnouncementStatusCodes.Publish);
		
		if (statusCode30 is null)
		{
			throw new NullReferenceException(nameof(statusCode30));
		}
		
		parameters.StatusId = statusCode30.Id;

		result =
			await AnnouncementService
				.GetAllWithPageAsync(parameters);

		return ToSampleResult(result);
	}

	#endregion /[HttpPost(template: "get-favorite")]

	#region [HttpPost(template: "recent-visit")]

	/// <summary>
	/// اخرین بازدیدها
	/// </summary>
	/// <param name="ids">لیست شناسه های مربوط به آیتم های اخیرا دیده شده</param>
	/// <returns></returns>
	
	[HttpPost(template: "recent-visit")]
	
	public async Task<IActionResult> ResentVisitAsync([FromBody] List<string>? ids)
	{
		var result = new Result<List<AnnouncementMiniResponseViewModel>>();
		
		if (ids is null || ids.Count == 0)
		{
			result.WithError(ResponseHelper.Response400WithCode(10));
			return ToSampleResult(result);
		}
		
		var parameters =
			new AnnouncementParameters
			{
				Ids = ids,
				PageSize = 50,
				PageNumber = 1,
				
				HasLiked = true,
				IsHidden = false,
				
				OrderBy = null,
				IsActive = true,
				IsDeleted = false,
			};

		var statusCode30 = await UnitOfWork
			.StatusRepository.FindByCodeAsync(AnnouncementStatusCodes.Publish);
		
		if (statusCode30 is null)
		{
			throw new NullReferenceException(nameof(statusCode30));
		}
		
		parameters.StatusId = statusCode30.Id;

		result =
			await AnnouncementService
				.ResentVisitAsync(parameters);
		
		return ToSampleResult(result);
	}

	#endregion /[HttpPost(template: "recent-visit")]
	
	#region [HttpPost(template: "use-map")]

	/// <summary>
	/// جهت استفاده از نقشه
	/// </summary>
	/// <param name="parameters"></param>
	/// <returns></returns>
	/// <exception cref="NullReferenceException"></exception>

	[HttpPost(template: "use-map")]

	public async Task<IActionResult> UseInMapAsync([FromBody] AnnouncementParameters? parameters)
	{
		var result = new Result<MapResponseViewModel>();

		if (parameters is null)
		{
			result.WithError(ResponseHelper.Response400WithCode(10));
			return ToSampleResult(result);
		}

		if (parameters.MapRequest is null)
		{
			result.WithError(ResponseHelper.Response400WithCode(20));
			return ToSampleResult(result);
		}
		
		parameters.IsHidden = false;

		parameters.IsActive = true;
		parameters.IsDeleted = false;

		parameters.PhoneNumber = null;
		
		var statusCode30 = await UnitOfWork
			.StatusRepository.FindByCodeAsync(AnnouncementStatusCodes.Publish);

		if (statusCode30 is null)
		{
			throw new NullReferenceException(nameof(statusCode30));
		}

		parameters.StatusId = statusCode30.Id;
		
		double zoom =
			parameters.MapRequest.Zoom;

		var value =
			new MapResponseViewModel
			{
				Zoom = zoom
			};

		var zoomString =
			Configuration.GetSection(key: ZoomKeyInConfiguration).Value;

		if (string.IsNullOrEmpty(zoomString) == true)
		{
			throw new NullReferenceException(nameof(zoomString));
		}

		var zoomConfig =
			Convert.ToInt32(zoomString);

		if (zoom < zoomConfig)
		{
			var clustersResult =
				await AnnouncementService
					.GetClustersAsync(parameters);

			if (clustersResult.IsSuccess == true)
			{
				value.Clusters = clustersResult.Value;
				value.CountAnnouncement = value.Clusters.Sum(x => x.Count);
			}
			else
			{
				result.WithErrors(clustersResult.Errors);
			}
		}
		else
		{
			var pagedResult =
				await AnnouncementService
					.GetAllWithPageAsync(parameters: parameters);

			if (pagedResult.IsSuccess == true)
			{
				value.Announcements = pagedResult.Value.Data;
				value.CountAnnouncement = value.Announcements.Count;
			}
			else
			{
				result.WithErrors(pagedResult.Errors);
			}
		}

		result.WithValue(value);

		return ToSampleResult(result);
	}

	#endregion /[HttpPost(template: "use-map")]

	#region [HttpGet(template: "get-announcements-with-notes")]

	/// <summary>
	/// دریافت لیست آگهی هایی که نوت دارند (با متن نوت)
	/// </summary>
	/// <returns></returns>
	[HttpGet(template: "get-announcements-with-notes")]
	public async Task<IActionResult> GetAnnouncementsWithNotesAsync(
		[FromQuery] AnnouncementParameters parameters)
	{
		var result = new Result<PagedListResult<AnnouncementMiniResponseViewModel>>();

		if (JwtTokenValidator.IsAuthenticated() == false)
		{
			result.WithError(Resources.ResponseErrors.UnauthorizedError401);
			return ToSampleResult(result);
		}
		
		parameters.IsDeleted = false;
		parameters.IsActive = true;
		parameters.MapRequest = null;
		
		parameters.IsHidden = false;

		parameters.IsActive = true;
		parameters.IsDeleted = false;

		parameters.PhoneNumber = null;

		parameters.ProfileNoteWriterId = JwtTokenValidator.GetUserId()!;
		parameters.OrderBy = AnnouncementOrderByFields.NoteDateDesc;
		
		result = await AnnouncementService
			.GetAnnouncementsWithNotesPopulatedAsync(parameters.ProfileNoteWriterId, parameters);

		return ToSampleResult(result);
	}

	#endregion /[HttpGet(template: "get-announcements-with-notes")]
	
	#region [HttpGet(template: "{id}")]

	/// <summary>
	/// دریافت آگهی با شناسه آن
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	[HttpGet(template: "{id}")]
	public async Task<IActionResult> GetByIdAsync([FromRoute] string id)
	{
		var userId = JwtTokenValidator.GetUserId();

		Result<AnnouncementResponseViewModel> result =
			await AnnouncementService.GetByIdAsync(id, userId);

		string ip = HttpContext.GetClientIp();

		if (result.IsSuccess == true)
		{
			var view =
				new AnnouncementViews
				{
					IpAddress = ip,
					ProfileId = userId,

					IsActive = true,
					IsDeleted = false,
					Ordering = 100_000,
					AnnouncementId = result.Value.Id!,
				};

			await UnitOfWork.AnnouncementViewsRepository.AddAsync(view);
			await UnitOfWork.SaveAsync();
		}

		return ToSampleResult(result);
	}

	#endregion /[HttpGet(template: "{id}")]

	#region [HttpPost(template: "get-by-profile-id/{profileId}")]

	/// <summary>
	/// دریافت لیست همه آگهی های افرادی که اجازه دسترسی داده اند به صورت صفحه بندی شده
	/// </summary>
	/// <param name="profileId">شناسه پروفایل اشخاص</param>
	/// <param name="parameters"></param>
	/// <returns></returns>
	[HttpPost(template: "get-by-profile-id/{profileId}")]
	public async Task<IActionResult> GetAllByProfileIdWithPageAsync(
		[FromRoute] string profileId,
		[FromQuery] AnnouncementParameters parameters)
	{
		var result =
			new Result<PagedListResult<AnnouncementMiniResponseViewModel>>();
		
		parameters.ProfileId = profileId;

		parameters.IsActive = true;
		parameters.IsDeleted = false;

		parameters.IsHidden = false;
		
		var statusCode30 = await UnitOfWork
			.StatusRepository.FindByCodeAsync(AnnouncementStatusCodes.Publish);

		if (statusCode30 is null)
		{
			throw new NullReferenceException(nameof(statusCode30));
		}

		parameters.StatusId = statusCode30.Id;

		var profile = await UnitOfWork
			.ProfileRepository.FindAsync(profileId);

		if (profile is null || profile.ShowProfileInAnnouncement == false)
		{
			result.WithError(Resources.ResponseErrors.ForbiddenError403);
			return ToSampleResult(result);
		}

		result =
			await AnnouncementService
				.GetAllWithPageAsync(parameters);

		return ToSampleResult(result);
	}

	#endregion /[HttpPost(template: "get-by-profile-id/{profileId}")]

	#region [HttpPost]

	/// <summary>
	/// ایجاد آگهی
	/// </summary>
	/// <param name="model"></param>
	/// <returns></returns>
	[HttpPost]
	public async Task<IActionResult> CreateAsync(
		[FromForm] AnnouncementRequestViewModel model)
	{
		var result = new Result<AnnouncementResponseViewModel>();

		var isAuthenticated = JwtTokenValidator.IsAuthenticated();

		if (isAuthenticated == false)
		{
			result.WithError(Resources.ResponseErrors.UnauthorizedError401);
			return ToSampleResult(result);
		}

		result = await AnnouncementService.CreateAsync(model);

		return ToSampleResult(result);
	}

	#endregion /[HttpPost]

	#region [HttpPut]

	/// <summary>
	/// ویرایش آگهی
	/// </summary>
	/// <param name="model"></param>
	/// <returns></returns>

	[HttpPut]
	
	public async Task<IActionResult> UpdateAsync([FromForm] AnnouncementUpdateRequestViewModel model)
	{
		var result = new Result();

		if (JwtTokenValidator.IsAuthenticated() == false)
		{
			result.WithError(Resources.ResponseErrors.UnauthorizedError401);
			return ToSampleResult(result);
		}

		string userId =
			JwtTokenValidator.GetUserId()!;

		result = await AnnouncementService.UpdateAsync(model, userId);

		return ToSampleResult(result);
	}

	#endregion /[HttpPut]
	
	#region [HttpPatch(template: "change-hidden")]

	/// <summary>
	/// تغییر وضعیت نمایش توسط کاربر
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	[HttpPatch(template: "change-hidden")]
	public async Task<IActionResult> ChangeIsHiddenAsync([FromQuery] string id)
	{
		var result = new Result<bool>();

		var tokenResult = JwtTokenValidator.GetUserId();
		
		if (string.IsNullOrEmpty(tokenResult) == true)
		{
			result.WithError(Resources.ResponseErrors.UnauthorizedError401);
			
			return ToSampleResult(result);
		}

		var profile =
			HttpContext.Items[key: nameof(Domain.Profile)] as Domain.Profile;

		result = await AnnouncementService
			.ChangeIsHiddenAsync(id, profile!.Id);

		return ToSampleResult(result);
	}

	#endregion /[HttpPatch(template: "change-hidden")]

	#region [HttpDelete]

	/// <summary>
	/// حذف آگهی با ثبت دلیل حذف توسط کاربر
	/// </summary>
	/// <param name="model"></param>
	/// <returns></returns>
	[HttpDelete]
	public async Task<IActionResult> DeleteAsync([FromBody] DeleteLogRequestViewModel model)
	{
		var result = new Result();

		if (JwtTokenValidator.IsAuthenticated() == false)
		{
			result.WithError(Resources.ResponseErrors.UnauthorizedError401);
			return ToSampleResult(result);
		}

		var userId = JwtTokenValidator.GetUserId();

		if (string.IsNullOrEmpty(userId) == true)
		{
			result.WithError(Resources.ResponseErrors.UnauthorizedError401);
			return ToSampleResult(result);
		}

		result = await AnnouncementService.DeleteAsync(model, userId);

		return ToSampleResult(result);
	}

	#endregion /[HttpDelete]
}