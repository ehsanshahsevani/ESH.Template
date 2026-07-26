using Domain;
using ESH.SeedworkSystem.Persistence;

namespace Persistence.Abstracts;

// For Note
public interface INoteRepository : IRepository<Note>
{
	/// <summary>
	/// دریافت تمام یادداشت های یک آگهی
	/// </summary>
	/// <param name="announcementId">شناسه آگهی</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<Note?> GetByAnnouncementIdAsync(string announcementId, CancellationToken cancellationToken = default);

	/// <summary>
	/// دریافت تمام یادداشت های یک کاربر
	/// </summary>
	/// <param name="profileId">شناسه کاربر</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<List<Note>> GetByProfileIdAsync(string profileId, CancellationToken cancellationToken = default);

	/// <summary>
	/// بررسی وجود یادداشت برای یک آگهی توسط یک کاربر
	/// </summary>
	/// <param name="announcementId">شناسه آگهی</param>
	/// <param name="profileId">شناسه کاربر</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<bool> ExistsAsync(string announcementId, string profileId, CancellationToken cancellationToken = default);

	/// <summary>
	/// دریافت یادداشت توسط آگهی و کاربر
	/// </summary>
	/// <param name="announcementId">شناسه آگهی</param>
	/// <param name="profileId">شناسه کاربر</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<List<Note>> GetByAnnouncementAndProfileAsync(
		string announcementId, string profileId, CancellationToken cancellationToken = default);
	Task<List<Note>> FindByAnnouncementAsync(
		string announcementId, string userId, CancellationToken cancellationToken = default);

	/// <summary>
	/// حذف دیتای این جدول برای یک کاربر در هنگام حذف حساب کاربری
	/// </summary>
	/// <param name="profileId"></param>
	/// <param name="cancellationToken"></param>
	Task DeleteAccountAsync(
		string profileId,
		CancellationToken cancellationToken = default);
}
