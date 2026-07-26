using Domain;
using ESH.BuildingBlocks.RequestFeatures;
using ESH.SeedworkSystem.Persistence;
using ESH.ViewModels.Announcement;
using ESH.ViewModels.Announcement.MapApp;
using ESH.ViewModels.Announcement.ModelParameters;


namespace Persistence.Abstracts;

// For Announcement
public interface IAnnouncementRepository : IRepository<Announcement>
{
	Task<AdminDashboardStatsViewModel> GetAdminDashboardStatsViewModel(string? statusId);
	Task<PagedList<Announcement>> GetAllWithPageAsync(
		AnnouncementParameters parameter,
		CancellationToken cancellationToken = default);
	Task<List<Announcement>> GetAnnouncementsByStatusIdAsync(int code, CancellationToken cancellationToken = default);
	Task<int> GetAnnouncementsCountAsync(string? statusId, DateTime startDate, DateTime endDate);
	Task<List<Domain.Announcement>> GetByIdsAsync(
		List<string> ids, CancellationToken cancellationToken = default);
	Task<Announcement?> GetByIdWithDetailsAsync(string id, CancellationToken cancellationToken = default);
	Task<int> GetCurrentMonthAnnouncementsCountAsync(string? statusId);
	Task<int> GetCurrentWeekAnnouncementsCountAsync(string? statusId);
	Task<int> GetCurrentYearAnnouncementsCountAsync(string? statusId);
	Task<int> GetTodayAnnouncementsCountAsync(string? statusId);

	Task<List<MapCluster>> GetClustersAsync(
		AnnouncementParameters parameter,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// حذف دیتای این جدول برای یک کاربر در هنگام حذف حساب کاربری
	/// </summary>
	/// <param name="profileId"></param>
	/// <param name="cancellationToken"></param>
	Task DeleteAccountAsync(
		string profileId,
		CancellationToken cancellationToken = default);

	Task<List<string>> GetIdsByCategoryIdsAsync(
		List<string> ids,
		int takeAnnouncement = 5,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// فیلتر / مرتب سازی
	/// - لیست بر میگرداند
	/// </summary>
	/// <param name="parameter"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<List<Announcement>> GetAllInListAsync(
		AnnouncementParameters parameter,
		CancellationToken cancellationToken = default);
}
