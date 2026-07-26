using Domain;

using Domain.Constants;
using ESH.BuildingBlocks.RequestFeatures;
using Persistence.Abstracts;
using ESH.Constant.Announcement;
using ESH.ViewModels.Announcement;
using Microsoft.EntityFrameworkCore;
using Persistence.Tools;
using ESH.ViewModels.Announcement.MapApp;
using ESH.ViewModels.Announcement.ModelParameters;

namespace Persistence.Repositories;

public class AnnouncementRepository : Repository<Announcement>, IAnnouncementRepository
{
	internal AnnouncementRepository(DatabaseContext databaseContext) : base(databaseContext)
	{
	}

	public override async Task<Announcement?> FindAsync(
		object id, bool? isActive = true, CancellationToken cancellationToken = default)
	{
		var result = await DbSet
			.Include(current => current.Status)
			.Include(current => current.Profile)
			.Where(current => current.Id == id.ToString())
			.FirstOrDefaultAsync(cancellationToken);

		return result;
	}

	public async Task<List<Domain.Announcement>> GetByIdsAsync(
		List<string> ids, CancellationToken cancellationToken = default)
	{
		var result = await DbSet
			.Include(current => current.Status)
			.Include(current => current.Profile)
			.Include(current => current.Category!)
			.ThenInclude(current => current.CategoryType)
			.Include(current => current.Profile)
			.Include(current => current.FieldValueAnnouncements)
			.ThenInclude(current => current.Field!)
			.ThenInclude(current => current.FieldType)
			.Where(current => current.IsDeleted == false)
			.Where(current => current.IsActive == true)
			.Where(current => ids.Contains(current.Id))
			.ToListAsync(cancellationToken);

		return result;
	}

	public async Task<Announcement?> GetByIdWithDetailsAsync(
		string id, CancellationToken cancellationToken = default)
	{
		var result = await DbSet
			.Include(current => current.Profile!)
			.ThenInclude(current => current.LanguageCode)
			.Include(current => current.Category!)
			.ThenInclude(current => current.CategoryType)
			.Include(current => current.Profile)
			.Include(current =>
				current.FieldValueAnnouncements.OrderBy(x => x.Field!.Ordering))
			.ThenInclude(current => current.Field!)
			.ThenInclude(current => current.FieldType)
			.Include(current =>
				current.Notes.Where(current => current.IsDeleted == false))
			.Where(current => current.IsDeleted == false)
			.Where(current => current.IsActive == true)
			.Where(current => current.Id == id)
			.FirstOrDefaultAsync(cancellationToken);

		if (result is not null)
		{
			result.FieldValueAnnouncements =
				result.FieldValueAnnouncements
					.Where(current => current.IsActive == true)
					.ToList();
		}

		return result;
	}

	/// <summary>
	/// فیلتر / مرتب سازی
	/// </summary>
	/// <param name="parameter"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<PagedList<Announcement>> GetAllWithPageAsync(
		AnnouncementParameters parameter,
		CancellationToken cancellationToken = default)
	{
		var source = DbSet
				.Include(current => current.Status)
				.Include(current => current.Profile)
				.Include(current => current.DictionaryChecker)
				.Include(current => current.DeleteReason)
				.Include(current => current.Category!)
				.ThenInclude(current => current.CategoryType)
				.Include(current => current.Profile)
				.Include(current => current.FieldValueAnnouncements)
				.ThenInclude(current => current.Field!)
				.ThenInclude(current => current.FieldType)
				.Include(current => current.Favorites)
				.AsQueryable()
			;

		source = await ApplyFilters(source, parameter, cancellationToken);

		var result =
			await PagedList<Announcement>.ToPagedList(
				source, parameter, cancellationToken);

		return result;
	}
	
	/// <summary>
	/// فیلتر / مرتب سازی
	/// - لیست بر میگرداند
	/// </summary>
	/// <param name="parameter"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<List<Announcement>> GetAllInListAsync(
		AnnouncementParameters parameter,
		CancellationToken cancellationToken = default)
	{
		var source = DbSet
				.Include(current => current.Status)
				.Include(current => current.Profile)
				.Include(current => current.DictionaryChecker)
				.Include(current => current.DeleteReason)
				.Include(current => current.Category!)
				.ThenInclude(current => current.CategoryType)
				.Include(current => current.Profile)
				.Include(current => current.FieldValueAnnouncements)
				.ThenInclude(current => current.Field!)
				.ThenInclude(current => current.FieldType)
				.Include(current => current.Favorites)
				.AsQueryable()
			;

		source = await ApplyFilters(source, parameter, cancellationToken);

		var result =
			await source.ToListAsync(cancellationToken);

		return result;
	}

	/// <summary>
	/// تعداد آگهی ثبت شده
	/// </summary>
	/// <param name="statusId">شناسه وضعیت آگهی</param>
	/// <param name="startDate">از تاریخ</param>
	/// <param name="endDate">تا تاریخ</param>
	/// <returns></returns>
	public async Task<int> GetAnnouncementsCountAsync(
		string? statusId,
		DateTime startDate,
		DateTime endDate)
	{
		var query = DbSet
			.Where(x => x.CreateDateTime >= startDate &&
			            x.CreateDateTime < endDate);

		if (statusId != null)
		{
			query = query
				.Where(x => x.StatusId == statusId);
		}

		int result =
			await query.CountAsync();

		return result;
	}

	/// <summary>
	/// تعداد آگهی ثبت شده امروز
	/// </summary>
	/// <param name="statusId"></param>
	/// <returns></returns>
	public async Task<int> GetTodayAnnouncementsCountAsync(string? statusId)
	{
		var today =
			DateTime.UtcNow.Date;

		var tomorrow =
			today.AddDays(1);

		int result = await GetAnnouncementsCountAsync(
			statusId,
			today,
			tomorrow);

		return result;
	}

	/// <summary>
	/// تعداد آگهی های ثبت شده در هفته جاری
	/// </summary>
	/// <param name="statusId">شناسه وضعیت</param>
	/// <returns></returns>
	public async Task<int> GetCurrentWeekAnnouncementsCountAsync(string? statusId)
	{
		var today =
			DateTime.UtcNow.Date;

		int diff =
			(int)today.DayOfWeek;

		var startOfWeek =
			today.AddDays(-diff);

		var nextWeek =
			startOfWeek.AddDays(7);

		int result = await GetAnnouncementsCountAsync(
			statusId,
			startOfWeek,
			nextWeek);

		return result;
	}

	/// <summary>
	/// تعداد آگهی های ماه جاری
	/// </summary>
	/// <param name="statusId">شناسه وضعیت</param>
	/// <returns></returns>
	public async Task<int> GetCurrentMonthAnnouncementsCountAsync(string? statusId)
	{
		var now =
			DateTime.UtcNow;

		var firstDay =
			new DateTime(now.Year, now.Month, 1);

		var nextMonth =
			firstDay.AddMonths(1);

		int result = await GetAnnouncementsCountAsync(
			statusId,
			firstDay,
			nextMonth);

		return result;
	}

	/// <summary>
	/// تعداد آگهی ها در سال جاری
	/// </summary>
	/// <param name="statusId">شناسه وضعیت</param>
	/// <returns></returns>
	public async Task<int> GetCurrentYearAnnouncementsCountAsync(string? statusId)
	{
		var now =
			DateTime.UtcNow;

		var firstDay =
			new DateTime(now.Year, 1, 1);

		var nextYear =
			firstDay.AddYears(1);

		int result = await GetAnnouncementsCountAsync(
			statusId,
			firstDay,
			nextYear);

		return result;
	}

	/// <summary>
	/// دریافت آمار آگهی، مربوط به صفحه اصلی پنل ادمین
	/// </summary>
	/// <param name="statusId">شناسه وضعیت آگهی</param>
	/// <returns></returns>
	public async Task<AdminDashboardStatsViewModel>
		GetAdminDashboardStatsViewModel(string? statusId)
	{
		IReportLogRepository reportLogRepository =
			new ReportLogRepository(DatabaseContext);

		var now = DateTime.UtcNow;
		var today = now.Date;

		var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
		var endOfWeek = startOfWeek.AddDays(7);

		var startOfMonth = new DateTime(now.Year, now.Month, 1);
		var endOfMonth = startOfMonth.AddMonths(1);

		var announcementsQuery = DbSet.AsQueryable();

		if (string.IsNullOrWhiteSpace(statusId) == false)
		{
			announcementsQuery = announcementsQuery.Where(x => x.StatusId == statusId);
		}

		// آمار آگهی‌ها
		var totalReportLog =
			await reportLogRepository.CountAsync();

		var totalAnnouncements =
			await announcementsQuery.CountAsync();

		var todayAnnouncements = await announcementsQuery
			.Where(x => x.CreateDateTime >= today && x.CreateDateTime < today.AddDays(1))
			.CountAsync();

		var weekAnnouncements = await announcementsQuery
			.Where(x => x.CreateDateTime >= startOfWeek && x.CreateDateTime < endOfWeek)
			.CountAsync();

		var monthAnnouncements = await announcementsQuery
			.Where(x => x.CreateDateTime >= startOfMonth && x.CreateDateTime < endOfMonth)
			.CountAsync();

		var announcementPending = await DbSet
			.Where(current => current.IsDeleted == false)
			.Where(current => current.Status!.Code == 10)
			.CountAsync();

		var announcementDictioanryChecker = await DbSet
			.Where(current => current.IsDeleted == false)
			.Where(current => current.HasWarningDictionaryChecker == true)
			.CountAsync();

		// آمار بازدیدها
		var viewsRepo = new AnnouncementViewsRepository(DatabaseContext);

		var totalViews =
			await viewsRepo.GetTotalViewsAsync();

		var todayViews =
			await viewsRepo.GetTodayTotalViewsAsync();

		var weekViews =
			await viewsRepo.GetDailyViewsAsync(startOfWeek, endOfWeek);

		var monthViews =
			await viewsRepo.GetDailyViewsAsync(startOfMonth, endOfMonth);

		var topCategories =
			await viewsRepo.GetTopViewedCategoriesAsync(5);

		var categoryIds =
			topCategories.Select(x => x.CategoryId).ToList();

		// دسته‌های برتر
		var categories = await announcementsQuery
			.Where(a => categoryIds.Contains(a.CategoryId))
			.GroupBy(a => a.CategoryId)
			.Select(g => new TopCategoryByAnnouncements
			{
				Count = g.Count(),
				ViewsCount = DatabaseContext
					.AnnouncementViews.Count(v => v.Announcement!.CategoryId == g.Key),
				Category = new CategoryResponseViewModel
				{
					Id = g.Key,
					IsActive = g.First().Category!.IsActive,
					Ordering = g.First().Category!.Ordering,
					ParentId = g.First().Category!.ParentId
				}
			})
			.OrderByDescending(x => x.Count)
			.Take(5)
			.ToListAsync();

		var result =
			new AdminDashboardStatsViewModel
			{
				WeekAnnouncements = weekAnnouncements,
				TotalAnnouncements = totalAnnouncements,
				TodayAnnouncements = todayAnnouncements,
				MonthAnnouncements = monthAnnouncements,
				MonthAnnouncementsReportsLog = totalReportLog,
				TotalAnnouncementsPending = announcementPending,
				TotalAnnouncementsDicktionaryChecker = announcementDictioanryChecker,

				TotalViews = totalViews,
				TodayViews = todayViews,

				WeekViews = weekViews.Sum(x => x.Count),
				MonthViews = monthViews.Sum(x => x.Count),

				TopCategoriesByAnnouncements = categories
			};

		return result;
	}

	#region Task<List<Announcement>> GetAnnouncementsByStatusIdAsync(int code)

	/// <summary>
	/// دریافت لیست مدل آگهی ها با استفاده از کد وضعیت
	/// </summary>
	/// <param name="code"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<List<Announcement>> GetAnnouncementsByStatusIdAsync(
		int code,
		CancellationToken cancellationToken = default)
	{
		var result = await DbSet
			.Where(current => current.IsActive == true)
			.Where(current => current.IsDeleted == false)
			.Where(current => current.Status!.Code == code)
			.ToListAsync(cancellationToken);

		return result;
	}

	#endregion /Task<List<Announcement>> GetAnnouncementsByStatusIdAsync(int code)

	#region Task<List<MapCluster>> GetClustersAsync(MapRequestViewModel request, AnnouncementParameters parameter)

	private double GetGridSizeByZoom(double zoom)
	{
		return zoom switch
		{
			<= 6 => 1.0,
			<= 8 => 0.5,
			<= 10 => 0.2,
			<= 12 => 0.05,
			_ => 0.02
		};
	}

	#endregion /Task<List<MapCluster>> GetClustersAsync(MapRequestViewModel request, AnnouncementParameters parameter)

	public async Task<List<MapCluster>> GetClustersAsync(
		AnnouncementParameters parameter,
		CancellationToken cancellationToken = default)
	{
		if (parameter.MapRequest is null)
		{
			throw new ArgumentNullException(nameof(parameter.MapRequest));
		}

		double gridSize = GetGridSizeByZoom(parameter.MapRequest.Zoom);

		var source = DbSet
			.Where(current => current.IsDeleted == false)
			.Where(current => current.IsActive == true)
			.AsQueryable();

		source = await ApplyFilters(source, parameter, cancellationToken);

		// مرحله ۱: خوشه‌بندی اولیه با grid
		var clusters = await source
			.GroupBy(a => new
			{
				LatIndex = Math.Floor(a.Latitude!.Value / gridSize),
				LonIndex = Math.Floor(a.Longitude!.Value / gridSize)
			})
			.Select(g => new MapCluster
			{
				Lat = g.Average(x => x.Latitude!.Value),
				Lon = g.Average(x => x.Longitude!.Value),
				Count = g.Count()
			})
			.ToListAsync(cancellationToken);

		var sumCluster =
			clusters.Sum(x => x.Count);

		if (clusters.Count > 10 && sumCluster is > 400 or < 300)
		{
			clusters = await MergeClustersAsync(clusters, 10, cancellationToken);
		}

		return clusters;
	}

	private async Task<List<MapCluster>> MergeClustersAsync(
		List<MapCluster> clusters,
		int maxClusters,
		CancellationToken cancellationToken)
	{
		while (clusters.Count > maxClusters)
		{
			// پیدا کردن نزدیک‌ترین جفت کلاستر
			double minDistance = double.MaxValue;
			int mergeIndex1 = -1, mergeIndex2 = -1;

			for (int i = 0; i < clusters.Count; i++)
			{
				for (int j = i + 1; j < clusters.Count; j++)
				{
					double distance = CalculateDistance(
						clusters[i].Lat, clusters[i].Lon,
						clusters[j].Lat, clusters[j].Lon);

					if (distance < minDistance)
					{
						minDistance = distance;
						mergeIndex1 = i;
						mergeIndex2 = j;
					}
				}
			}

			// ادغام دو کلاستر نزدیک
			if (mergeIndex1 != -1 && mergeIndex2 != -1)
			{
				var merged = new MapCluster
				{
					Lat = (clusters[mergeIndex1].Lat * clusters[mergeIndex1].Count +
					       clusters[mergeIndex2].Lat * clusters[mergeIndex2].Count) /
					      (clusters[mergeIndex1].Count + clusters[mergeIndex2].Count),

					Lon = (clusters[mergeIndex1].Lon * clusters[mergeIndex1].Count +
					       clusters[mergeIndex2].Lon * clusters[mergeIndex2].Count) /
					      (clusters[mergeIndex1].Count + clusters[mergeIndex2].Count),

					Count = clusters[mergeIndex1].Count + clusters[mergeIndex2].Count
				};

				clusters.RemoveAt(Math.Max(mergeIndex1, mergeIndex2));
				clusters.RemoveAt(Math.Min(mergeIndex1, mergeIndex2));
				clusters.Add(merged);
			}
		}

		return clusters;
	}

	private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
	{
		// فرمول هاورسین برای فاصله بر حسب کیلومتر
		var R = 6371.0; // شعاع زمین
		var dLat = ToRadians(lat2 - lat1);
		var dLon = ToRadians(lon2 - lon1);

		var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
		        Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
		        Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

		var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
		return R * c;
	}

	private double ToRadians(double degrees) => degrees * Math.PI / 180;


	#region ApplyFilter

	private async Task<IQueryable<Domain.Announcement>> ApplyFilters(
		IQueryable<Announcement> source, AnnouncementParameters parameters,
		CancellationToken cancellationToken = default)
	{
		if (parameters.IsDeleted.HasValue == true)
		{
			source = source.Where(current => current.IsDeleted == parameters.IsDeleted.Value);
		}

		if (parameters.IsActive.HasValue == true)
		{
			source = source.Where(current => current.IsActive == parameters.IsActive.Value);
		}

		if (parameters.MapRequest is not null &&
		    parameters.MapRequest.ViewportPolygon != null &&
		    parameters.MapRequest.ViewportPolygon.Count == 4)
		{
			// مرحله 1: فیلتر سریع با Bounding Box (روی دیتابیس)
			var minLat = parameters.MapRequest.ViewportPolygon.Min(p => p.Lat);
			var maxLat = parameters.MapRequest.ViewportPolygon.Max(p => p.Lat);
			var minLon = parameters.MapRequest.ViewportPolygon.Min(p => p.Lon);
			var maxLon = parameters.MapRequest.ViewportPolygon.Max(p => p.Lon);

			source = source
				.Where(current => current.Latitude.HasValue)
				.Where(current => current.Longitude.HasValue == true)
				.Where(a =>
					a.Latitude >= minLat && a.Latitude <= maxLat &&
					a.Longitude >= minLon && a.Longitude <= maxLon
				);
		}

		if (parameters.BlurPlateLetters.HasValue == true)
		{
			source = source
				.Where(current =>
					current.BlurPlateLetters == parameters.BlurPlateLetters);
		}

		if (parameters.HasWarningDictionaryChecker.HasValue == true)
		{
			source = source.Where(current =>
				current.HasWarningDictionaryChecker == parameters.HasWarningDictionaryChecker.Value);
		}

		if (string.IsNullOrEmpty(parameters.DeleteReasonId) == false)
		{
			source = source.Where(current => current.DeleteReasonId == parameters.DeleteReasonId);
		}

		if (string.IsNullOrEmpty(parameters.DictionaryCheckerId) == false)
		{
			source = source.Where(current => current.DictionaryCheckerId == parameters.DictionaryCheckerId);
		}

		if (string.IsNullOrEmpty(parameters.ReportReasonId) == false)
		{
			var ids = await DatabaseContext.ReportLogs
				.Where(current => current.ReportReasonId == parameters.ReportReasonId)
				.Select(current => current.AnnouncementId)
				.ToListAsync(cancellationToken);

			parameters.Ids.AddRange(ids);

			parameters.Ids =
				parameters.Ids.Distinct().ToList();
		}

		if (string.IsNullOrEmpty(parameters.CategoryId) == false)
		{
			source = source.Where(current => current.CategoryId == parameters.CategoryId);
		}

		if (string.IsNullOrEmpty(parameters.StatusId) == false)
		{
			source = source.Where(current => current.StatusId == parameters.StatusId);
		}

		if (string.IsNullOrEmpty(parameters.PhoneNumber) == false)
		{
			source = source
				.Where(current => current.Profile!.FullPhoneNumber.Contains(parameters.PhoneNumber));
		}

		if (parameters.IsHidden.HasValue == true)
		{
			source = source
				.Where(current => current.IsHidden == parameters.IsHidden.Value);

			IStatusRepository statusRepository = new StatusRepository(DatabaseContext);
			
			var rejected = await statusRepository
				.FindByCodeAsync(AnnouncementStatusCodes.Rejected, cancellationToken);
			
			var expired =  await statusRepository
				.FindByCodeAsync(AnnouncementStatusCodes.Expired, cancellationToken);

			List<string> listIds = [expired!.Id, rejected!.Id];

			source = source.Where(current => listIds.Contains(current.StatusId) == false);
		}

		if (string.IsNullOrEmpty(parameters.Text) == false)
		{
			var ids = await DatabaseContext
				.FieldValueAnnouncements
				.Where(x => x.Value!.Contains(parameters.Text))
				.Select(current => current.AnnouncementId)
				.ToListAsync(cancellationToken);

			List<string> subSystems =
			[
				nameof(Domain.Announcement)
			];

			if (ids.Any() == true)
			{
				ids.AddRange(
					await DatabaseContext.LanguageLocalizers
						.Include(current => current.SubSystem)
						.Where(current => subSystems.Contains(current.SubSystem.Name))
						.Where(current => current.Value.Contains(parameters.Text))
						.Select(current => current.RelationId)
						.ToListAsync(cancellationToken)
				);

				source = source.Where(current => ids.Contains(current.Id) == true
				                                 || ids.Contains(current.CategoryId) == true);
			}

			parameters.RegionIds.AddRange(
				await DatabaseContext.LanguageLocalizers
					.Include(current => current.SubSystem)
					.Where(current => current.SubSystem.Name == nameof(Domain.Region))
					.Where(current => current.Value.Contains(parameters.Text))
					.Select(current => current.RelationId)
					.ToListAsync(cancellationToken)
			);

			parameters.RegionIds =
				parameters.RegionIds.Distinct().ToList();

			parameters.PhoneOperatorIds.AddRange(
				await DatabaseContext.LanguageLocalizers
					.Include(current => current.SubSystem)
					.Where(current => current.SubSystem.Name == nameof(Domain.PhoneOperator))
					.Where(current => current.Value.Contains(parameters.Text))
					.Select(current => current.RelationId)
					.ToListAsync(cancellationToken)
			);

			parameters.PhoneOperatorIds =
				parameters.PhoneOperatorIds.Distinct().ToList();

			if (parameters.Ids.Any() == false
			    && ids.Any() == false
			    && parameters.RegionIds.Any() == false
			    && parameters.PhoneOperatorIds.Any() == false)
			{
				source = source.Take(0);
			}
		}

		if (parameters.FieldMultiValueIds.Any() == true)
		{
			var fieldMultivalueIds = parameters.FieldMultiValueIds;

			source = source.Where(current => current.FieldValueAnnouncements
				.Any(fv => fv.Field != null
				           && fv.Field.FieldType != null
				           && fv.Field.FieldType.Code == FieldTypes.CustomValues
				           && fieldMultivalueIds.Contains(fv.Value!)));
		}

		if (parameters.RegionIds.Any() == true)
		{
			var regionIds = parameters.RegionIds;

			source = source.Where(current => current.FieldValueAnnouncements
				.Any(fv => fv.Field != null
				           && fv.Field.FieldType != null
				           && fv.Field.FieldType.Code == FieldTypes.Region
				           && regionIds.Contains(fv.Value!)));
		}

		if (parameters.PhoneOperatorIds.Any() == true)
		{
			var operatorIds = parameters.PhoneOperatorIds;

			source = source.Where(current => current.FieldValueAnnouncements
				.Any(fv => fv.Field != null
				           && fv.Field.FieldType != null
				           && fv.Field.FieldType.Code == FieldTypes.PhoneOperator
				           && operatorIds.Contains(fv.Value!)));
		}

		if (parameters.PlateStatusIds.Any() == true)
		{
			var plateStatusIds = parameters.PlateStatusIds;

			source = source.Where(current => current.FieldValueAnnouncements
				.Any(fv => fv.Field != null
				           && fv.Field.FieldType != null
				           && fv.Field.FieldType.Code == FieldTypes.PlateStatus
				           && plateStatusIds.Contains(fv.Value!)));
		}

		if (parameters.PlateLetterIds.Any() == true)
		{
			var plateLetterIds = parameters.PlateLetterIds;

			source = source
				.Where(current => current.FieldValueAnnouncements
					.Any(fv => fv.Field != null
					           && fv.Field.FieldType != null
					           && fv.Field.FieldType.Code == FieldTypes.PlateLetter
					           && plateLetterIds.Contains(fv.Value!)))
				.Where(current => current.BlurPlateLetters == false);

			;
		}

		if (string.IsNullOrEmpty(parameters.ReportOwnerId) == false)
		{
			var reportIdsByThisProfileId =
				await DatabaseContext.ReportLogs
					.Where(current => current.IsDeleted == false)
					.Where(current => current.IsActive == true)
					.Where(current => current.ProfileId == parameters.ReportOwnerId)
					.Select(current => current.AnnouncementId)
					.ToListAsync(cancellationToken);

			parameters.Ids.AddRange(reportIdsByThisProfileId);

			parameters.Ids =
				parameters.Ids.Distinct().ToList();
		}

		if (string.IsNullOrEmpty(parameters.ReportedUserId) == false)
		{
			var reportIdsByThisProfileId =
				await DatabaseContext.ReportLogs
					.Where(current => current.IsDeleted == false)
					.Where(current => current.IsActive == true)
					.Where(current => current.Announcement.ProfileId == parameters.ReportedUserId)
					.Select(current => current.AnnouncementId)
					.ToListAsync(cancellationToken);

			parameters.Ids.AddRange(reportIdsByThisProfileId);

			parameters.Ids =
				parameters.Ids.Distinct().ToList();
		}

		if (parameters.Ids.Any() == true)
		{
			var ids = parameters.Ids;

			source = source
				.Where(current => ids.Contains(current.Id));
		}

		if (string.IsNullOrEmpty(parameters.ProfileId) == false)
		{
			source = source
				.Where(current => current.ProfileId == parameters.ProfileId);
		}

		if (parameters.HasLiked.HasValue == true
		    && string.IsNullOrEmpty(parameters.ProfileFavoriteId) == false)
		{
			var announcementIds = await DatabaseContext.Favorites
				.Where(favorite => favorite.ProfileId == parameters.ProfileFavoriteId)
				.Where(favorite => favorite.IsDeleted == false)
				.Where(favorite => favorite.Announcement.IsDeleted == false)
				.Where(favorite => favorite.Announcement.IsActive == true)
				.Where(favorite => favorite.Announcement.IsHidden == false)
				.Select(current => current.AnnouncementId)
				.ToListAsync(cancellationToken);

			source = source
				.Where(announcement => announcementIds
					.Distinct().Contains(announcement.Id));
		}

		if (parameters.MinPrice.HasValue == true)
		{
			var minPrice = parameters.MinPrice.Value;

			source = source.Where(announcement => announcement.Price >= minPrice);
		}

		if (parameters.MaxPrice.HasValue == true)
		{
			var maxPrice = parameters.MaxPrice.Value;

			source = source.Where(announcement => announcement.Price <= maxPrice);
		}

		if (parameters.CountPlateLetters.Any() == true)
		{
			var plateLetterIds =
				await DatabaseContext.PlateCodes
					.Where(current => parameters.CountPlateLetters.Contains(current.EnUs.Length))
					.Select(x => x.Id)
					.ToListAsync(cancellationToken);

			source = source
					.Where(announcement =>
						announcement.Category!.CategoryType!.Code == CategoryTypes.Plate
						&& announcement.FieldValueAnnouncements
							.Where(valueAnnouncement => plateLetterIds.Contains(valueAnnouncement.Value!))
							.Any()
					)
					.Where(current => current.BlurPlateLetters == false)
				;
		}

		if (parameters.CountPlateNumbers.Any() == true)
		{
			source = source
				.Where(announcement =>
					announcement.Category!.CategoryType!.Code == CategoryTypes.Plate
					&& announcement.FieldValueAnnouncements
						.Where(valueAnnouncement =>
							valueAnnouncement.Field!.FieldType!.Code == FieldTypes.PlateNumberPart)
						.Where(valueAnnouncement =>
							parameters.CountPlateNumbers.Contains(valueAnnouncement.Value!.Length))
						.Any()
				);
		}

		if (parameters.PlateNumberTypes.Any() == true)
		{
			var listPlates =
				await DatabaseContext.FieldValueAnnouncements
					.Include(current => current.Field!)
					.ThenInclude(current => current.FieldType)
					.Where(current => current.Field!.FieldType!.Code == FieldTypes.PlateNumberPart)
					.Select(current => current.Value)
					.ToListAsync(cancellationToken);

			List<string> plateNumbers = [];

			foreach (var plateNumberType in parameters.PlateNumberTypes)
			{
				switch (plateNumberType)
				{
					case PlateNumberTypes.ORDER:
					{
						plateNumbers.AddRange(
							listPlates
								.Where(current => AnnouncementMethods.HasConsecutiveNumbers(current!) == true)
								.Select(current => current!.ToString())
								.ToList()
						);

						break;
					}
					case PlateNumberTypes.BIRTH_DATE:
					{
						var startYear = 1950;
						var year = DateTime.Now.Year + 10;

						plateNumbers.AddRange(
							listPlates
								.Select(current => Convert.ToInt32(current))
								.Where(current => current >= startYear)
								.Where(current => current <= year)
								.Select(current => current.ToString())
								.ToList()
						);

						break;
					}
					case PlateNumberTypes.REPEAT:
					{
						plateNumbers.AddRange(
							listPlates
								.Where(current => AnnouncementMethods.HasRepeatedDigits(current!) == true)
								.Select(current => current!.ToString())
								.ToList()
						);

						break;
					}
					case PlateNumberTypes.SPAN:
					{
						plateNumbers.AddRange(
							listPlates
								.Where(current => AnnouncementMethods.IsPalindromeBetween(current!) == true)
								.Select(current => current!.ToString())
								.ToList()
						);

						break;
					}
				}
			}

			source = source
				.Where(announcement =>
					announcement.Category!.CategoryType!.Code == CategoryTypes.Plate
					&& announcement.FieldValueAnnouncements
						.Where(valueAnnouncement =>
							valueAnnouncement.Field!.FieldType!.Code == FieldTypes.PlateNumberPart)
						.Where(valueAnnouncement => plateNumbers.Contains(valueAnnouncement.Value!))
						.Any()
				);
		}

		if (parameters.PhoneNumberTypes.Any() == true)
		{
			var phoneNumbersEntities =
				await DatabaseContext.FieldValueAnnouncements
					.Include(current => current.Field!)
					.ThenInclude(current => current.FieldType)
					.Where(current => current.Field!.FieldType!.Code == FieldTypes.PhoneBody)
					.Select(current => current.Value)
					.ToListAsync(cancellationToken);

			List<string> phoneNumbers = [];

			foreach (var phoneNumberType in parameters.PhoneNumberTypes)
			{
				switch (phoneNumberType)
				{
					case PhoneNumberTypes.TWO:
					{
						phoneNumbers.AddRange(
							phoneNumbersEntities
								.Select(current => current!)
								.Where(current => current[1] == current[2])
								.ToList()
						);

						break;
					}
					case PhoneNumberTypes.THREE:
					{
						phoneNumbers.AddRange(
							phoneNumbersEntities
								.Select(current => current!)
								.Where(current => current[1] == current[2])
								.Where(current => current[2] == current[3])
								.ToList()
						);

						break;
					}
					case PhoneNumberTypes.FOUR:
					{
						phoneNumbers.AddRange(
							phoneNumbersEntities
								.Select(current => current!)
								.Where(current => current[1] == current[2])
								.Where(current => current[2] == current[3])
								.Where(current => current[3] == current[4])
								.ToList()
						);

						break;
					}
					case PhoneNumberTypes.FIVE:
					{
						phoneNumbers.AddRange(
							phoneNumbersEntities
								.Select(current => current!)
								.Where(current => current[1] == current[2])
								.Where(current => current[2] == current[3])
								.Where(current => current[3] == current[4])
								.Where(current => current[4] == current[5])
								.ToList()
						);

						break;
					}
					case PhoneNumberTypes.SIX:
					{
						phoneNumbers.AddRange(
							phoneNumbersEntities
								.Select(current => current!)
								.Where(current => current[1] == current[2])
								.Where(current => current[2] == current[3])
								.Where(current => current[3] == current[4])
								.Where(current => current[4] == current[5])
								.Where(current => current[5] == current[6])
								.ToList()
						);

						break;
					}
					case PhoneNumberTypes.SEVEN:
					{
						phoneNumbers.AddRange(
							phoneNumbersEntities
								.Select(current => current!)
								.Where(current => current[1] == current[2])
								.Where(current => current[2] == current[3])
								.Where(current => current[3] == current[4])
								.Where(current => current[4] == current[5])
								.Where(current => current[5] == current[6])
								.Where(current => current[6] == current[7])
								.ToList()
						);

						break;
					}
				}
			}

			source = source
				.Where(announcement =>
					announcement.Category!.CategoryType!.Code == CategoryTypes.Phone
					&& announcement.FieldValueAnnouncements
						.Where(valueAnnouncement => valueAnnouncement.Field!.FieldType!.Code == FieldTypes.PhoneBody)
						.Where(valueAnnouncement => phoneNumbers.Contains(valueAnnouncement.Value!))
						.Any()
				);
		}

		if (string.IsNullOrEmpty(parameters.OrderBy) == false)
		{
			switch (parameters.OrderBy)
			{
				case AnnouncementOrderByFields.CreateDateTimeOrderBy:
				{
					source = source
							.OrderBy(current => current.Ordering)
							.ThenBy(announcement => announcement.UpdateDateTime)
						;
					break;
				}
				case AnnouncementOrderByFields.CreateDateTimeOrderByDesc:
				{
					source = source
							.OrderBy(current => current.Ordering)
							.ThenByDescending(current => current.UpdateDateTime)
						;
					break;
				}
				case AnnouncementOrderByFields.PriceOrderBy:
				{
					source = source
							.OrderBy(current => current.Ordering)
							.ThenBy(current => current.Price)
						;
					break;
				}
				case AnnouncementOrderByFields.PriceOrderByDesc:
				{
					source = source
							.OrderBy(current => current.Ordering)
							.ThenByDescending(current => current.Price)
						;
					break;
				}
				case AnnouncementOrderByFields.FavoriteDateDesc:
				{
					var profileId = parameters.ProfileFavoriteId;

					if (string.IsNullOrEmpty(profileId) == true)
					{
						throw new NullReferenceException("parameters.ProfileFavoriteId");
					}
					else
					{
						source = source
							.GroupJoin(
								DatabaseContext.Favorites
									.Where(x => x.IsDeleted == false)
									.Where(x => x.Announcement.IsDeleted == false)
									.Where(x => x.Announcement.IsActive == true)
									.Where(x => x.Announcement.IsHidden == false)
								,
								announcement => new { AnnouncementId = announcement.Id, ProfileId = profileId },
								favorite => new { favorite.AnnouncementId, favorite.ProfileId },
								(announcement, favGroup) => new { announcement, favGroup = favGroup }
							)
							.SelectMany(
								x => x.favGroup.DefaultIfEmpty(),
								(x, fav) => new { x.announcement, fav }
							)
							.OrderByDescending(x => x.fav.CreateDateTime)
							.ThenBy(x => x.announcement.Ordering)
							.ThenByDescending(x => x.announcement.CreateDateTime)
							.Select(x => x.announcement);
					}

					break;
				}
				case AnnouncementOrderByFields.NoteDateDesc:
				{
					var profileId = parameters.ProfileNoteWriterId;
					if (string.IsNullOrEmpty(profileId))
					{
						source = source
							.OrderBy(a => a.Ordering)
							.ThenByDescending(a => a.CreateDateTime);
					}
					else
					{
						// استخراج آخرین تاریخ نوت برای هر آگهی (برای کاربر جاری)
						var latestNotePerAnnouncement = DatabaseContext.Notes
							.Where(n => n.ProfileId == profileId)
							.Where(n => n.IsDeleted == false)
							.Where(n => n.Announcement.IsHidden == false)
							.Where(n => n.ProfileId == profileId)
							.GroupBy(n => n.AnnouncementId)
							.Select(g => new { AnnouncementId = g.Key, LatestDate = g.Max(n => n.CreateDateTime) });

						source = from announcement in source
							join note in latestNotePerAnnouncement on announcement.Id equals note.AnnouncementId into
								noteJoin
							from nj in noteJoin.DefaultIfEmpty()
							orderby nj.LatestDate descending, announcement.Ordering, announcement.CreateDateTime
								descending
							select announcement;
					}

					break;
				}
			}
		}
		else
		{
			source = source
					.OrderBy(current => current.Ordering)
					.ThenByDescending(current => current.CreateDateTime)
				;
		}

		return source;
	}

	#endregion /ApplyFilter

	#region DeleteAccountAsync(string profileId)

	/// <summary>
	/// حذف دیتای این جدول برای یک کاربر در هنگام حذف حساب کاربری
	/// </summary>
	/// <param name="profileId"></param>
	/// <param name="cancellationToken"></param>
	public async Task DeleteAccountAsync(
		string profileId,
		CancellationToken cancellationToken = default)
	{
		var ids = await DbSet
			.Where(x => x.ProfileId == profileId)
			.Select(x => x.Id)
			.ToListAsync(cancellationToken);

		if (ids.Any() == true)
		{
			await DatabaseContext.Attachments
				.Where(x => x.SubSystem.Name == nameof(Domain.Announcement))
				.Where(x => ids.Contains(x.RelationId))
				.ExecuteDeleteAsync(cancellationToken);

			await DbSet
				.Where(x => ids.Contains(x.Id))
				.ExecuteDeleteAsync(cancellationToken);
		}
	}

	#endregion /DeleteAccountAsync(string profileId)
	
	public async Task<List<string>> GetIdsByCategoryIdsAsync(
		List<string> ids,
		int takeAnnouncement = 5,
		CancellationToken cancellationToken = default)
	{
		var query = DbSet
			.Where(a => ids.Contains(a.CategoryId))
			.Select(a => new
			{
				a.Id,
				Rank = DbSet.Count(x => x.CategoryId == a.CategoryId &&
				                        x.CreateDateTime> a.CreateDateTime) + 1
			})
			.Where(x => x.Rank <= takeAnnouncement)
			.Select(x => x.Id);

		var result = await query.ToListAsync(cancellationToken);
		return result;
	}
}