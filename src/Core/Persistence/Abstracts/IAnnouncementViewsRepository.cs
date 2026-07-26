using Domain;
using Persistence.Repositories;
using ESH.SeedworkSystem.Persistence;

namespace Persistence.Abstracts;

public interface IAnnouncementViewsRepository : IRepository<AnnouncementViews>
{
	Task<int> GetCurrentMonthViewsAsync();
	Task<List<DailyViewStat>> GetDailyViewsAsync(string announcementId, DateTimeOffset startDate, DateTimeOffset endDate);
	Task<List<DailyViewStat>> GetDailyViewsAsync(DateTimeOffset startDate, DateTimeOffset endDate);
	Task<int> GetTodayTotalViewsAsync();
	Task<int> GetTodayViewsAsync(string announcementId);
	Task<int> GetTodayViewsAsync();
	Task<List<TopAnnouncementViewModel>> GetTopViewedAnnouncementsAsync(int count);
	Task<List<TopCategoryViewModel>> GetTopViewedCategoriesAsync(int count);
	Task<int> GetTotalViewsAsync(string announcementId);
	Task<int> GetTotalViewsAsync();
	Task<int> GetTotalViewsByCategoryAsync(string categoryId);

	/// <summary>
	/// حذف دیتای این جدول برای یک کاربر در هنگام حذف حساب کاربری
	/// </summary>
	/// <param name="profileId"></param>
	/// <param name="cancellationToken"></param>
	Task DeleteAccountAsync(
		string profileId,
		CancellationToken cancellationToken = default);
}