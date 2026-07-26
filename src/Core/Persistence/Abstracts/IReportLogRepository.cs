
using Domain;
using ESH.BuildingBlocks.RequestFeatures;
using ESH.SeedworkSystem.Persistence;
using ESH.ViewModels.Announcement.ModelParameters;


namespace Persistence.Abstracts;

// For ReportLog
public interface IReportLogRepository : IRepository<ReportLog>
{
	Task<int> CountAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// بررسی اینکه آیا یک گزارش قبلا توسط کاربر برای این آگهی ثبت شده است یا نه
	/// </summary>
	/// <param name="announcementId">شناسه آگهی</param>
	/// <param name="profileId">شناسه کاربر</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<bool> ExistsByAnnouncementAndProfileAsync(string announcementId, string profileId, CancellationToken cancellationToken = default);
	Task<PagedList<ReportLog>> GetAllWithPageAsync(ReportLogParameters parameter, CancellationToken cancellationToken = default);
	Task<List<string>> GetAnnouncementIdsAsync();

	/// <summary>
	/// حذف دیتای این جدول برای یک کاربر در هنگام حذف حساب کاربری
	/// </summary>
	/// <param name="profileId"></param>
	/// <param name="cancellationToken"></param>
	Task DeleteAccountAsync(
		string profileId,
		CancellationToken cancellationToken = default);
}
