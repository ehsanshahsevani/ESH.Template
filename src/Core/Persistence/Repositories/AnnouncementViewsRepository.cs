using ESH.BuildingBlocks.SampleResult;
using Domain;
using Persistence.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class AnnouncementViewsRepository
	: Repository<AnnouncementViews>, IAnnouncementViewsRepository
{
	internal AnnouncementViewsRepository(DatabaseContext databaseContext)
		: base(databaseContext)
	{
	}

	public async Task<int> GetTotalViewsAsync(string announcementId)
	{
		int result = await DbSet
			.Where(x => x.AnnouncementId == announcementId)
			.CountAsync();

		return result;
	}

	public async Task<int> GetTotalViewsAsync()
	{
		int result = await DbSet.CountAsync();

		return result;
	}

	public async Task<int> GetTotalViewsByCategoryAsync(string categoryId)
	{
		int result = await DbSet

			.Where(current => current.Announcement!.CategoryId == categoryId)

			.CountAsync();

		return result;
	}

	public async Task<int> GetTodayViewsAsync()
	{
		var today = DateTime.UtcNow.Date;

		int result = await DbSet

			.Where(x => x.CreateDateTime >= today &&
						x.CreateDateTime < today.AddDays(1))

			.CountAsync();

		return result;
	}

	public async Task<int> GetTodayViewsAsync(string announcementId)
	{
		var today = DateTime.UtcNow.Date;

		int result = await DbSet

			.Where(x => x.AnnouncementId == announcementId)

			.Where(x => x.CreateDateTime >= today &&
						x.CreateDateTime < today.AddDays(1))

			.CountAsync();

		return result;
	}

	public async Task<List<DailyViewStat>> GetDailyViewsAsync(
		string announcementId,
		DateTimeOffset startDate,
		DateTimeOffset endDate)
	{
		List<DailyViewStat> result = await DbSet

			.Where(x => x.AnnouncementId == announcementId)

			.Where(x => x.CreateDateTime >= startDate &&
						x.CreateDateTime <= endDate)

			.GroupBy(x => x.CreateDateTime.Date)

			.Select(g => new DailyViewStat
			{
				Date = g.Key,
				Count = g.Count()
			})

			.OrderBy(x => x.Date)
			.ToListAsync();

		return result;
	}

	public async Task<List<DailyViewStat>> GetDailyViewsAsync(
		DateTimeOffset startDate,
		DateTimeOffset endDate)
	{
		List<DailyViewStat> result = await DbSet

			.Where(x => x.CreateDateTime >= startDate &&
						x.CreateDateTime <= endDate)

			.GroupBy(x => x.CreateDateTime.Date)

			.Select(g => new DailyViewStat
			{
				Date = g.Key,
				Count = g.Count()
			})

			.OrderBy(x => x.Date)
			.ToListAsync();

		return result;
	}

	public async Task<List<TopAnnouncementViewModel>> GetTopViewedAnnouncementsAsync(int count)
	{
		List<TopAnnouncementViewModel> result = await DbSet

			.GroupBy(x => x.AnnouncementId)

			.Select(g => new TopAnnouncementViewModel
			{
				AnnouncementId = g.Key,
				TotalViews = g.Count()
			})

			.OrderByDescending(x => x.TotalViews)

			.Take(count)

			.ToListAsync();

		return result;
	}

	public async Task<List<TopCategoryViewModel>> GetTopViewedCategoriesAsync(int count)
	{
		List<TopCategoryViewModel> result = await DbSet

			.GroupBy(x => x.Announcement!.CategoryId)

			.Select(g => new TopCategoryViewModel
			{
				CategoryId = g.Key,
				TotalViews = g.Count()
			})

			.OrderByDescending(x => x.TotalViews)

			.Take(count)

			.ToListAsync();

		return result;
	}

	public async Task<int> GetTodayTotalViewsAsync()
	{
		var today =
			DateTime.UtcNow.Date;

		return await DbSet

			.Where(x => x.CreateDateTime >= today &&
						x.CreateDateTime < today.AddDays(1))
			
			.CountAsync();
	}

	public async Task<int> GetCurrentMonthViewsAsync()
	{
		var now = DateTime.UtcNow;

		var firstDay =
			new DateTime(now.Year, now.Month, 1);
		
		var nextMonth =
			firstDay.AddMonths(1);

		int result = await DbSet

			.Where(x => x.CreateDateTime >= firstDay &&
						x.CreateDateTime < nextMonth)

			.CountAsync();

		return result;
	}

	public override async Task<ESH.BuildingBlocks.SampleResult.Result> AddAsync(AnnouncementViews? entity, CancellationToken cancellationToken = default)
	{
		var result = new Result();

		if (entity is null)
		{
			return result;
		}

		var now = DateTime.UtcNow;
		var limit = now.AddMinutes(-5);

		var exists = await DbSet

			.Where(current => current.AnnouncementId == entity.AnnouncementId)
			.Where(current => current.IpAddress == entity.IpAddress)
			.Where(current => current.CreateDateTime >= limit)

			.AnyAsync(cancellationToken);

		if (exists == true)
		{
			return result;
		}

		return await base.AddAsync(entity, cancellationToken);
	}
	
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
		var result = await DbSet
			.Where(x => x.ProfileId == profileId)
			.ToListAsync(cancellationToken);

		foreach (var item in result)
		{
			item.ProfileId = null;
			item.UpdateDateTime = ESH.Utilities.DateTools.DateTimeNow();
		}

		await DbSet
			.Where(current => current.Announcement!.ProfileId == profileId)
			.ExecuteDeleteAsync(cancellationToken);
	}
	
	#endregion /DeleteAccountAsync(string profileId)
}

public class DailyViewStat : object
{
	public DailyViewStat() : base()
	{
	}

	public int Count { get; set; }
	public DateTimeOffset Date { get; set; }
}

public class TopAnnouncementViewModel : object
{
	public TopAnnouncementViewModel() : base()
	{
	}

	public int TotalViews { get; set; }
	public string AnnouncementId { get; set; }
}

public class TopCategoryViewModel : object
{
	public TopCategoryViewModel() : base()
	{
	}

	public int TotalViews { get; set; }
	public string CategoryId { get; set; }
}
