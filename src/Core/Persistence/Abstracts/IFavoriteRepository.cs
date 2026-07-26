using Domain;
using ESH.SeedworkSystem.Persistence;

namespace Persistence.Abstracts;

// For Favorite
public interface IFavoriteRepository : IRepository<Favorite>
{
	/// <summary>
	/// دریافت تمام علاقمندی های یک کاربر
	/// </summary>
	/// <param name="profileId">شناسه کاربر</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<List<Favorite>> GetByProfileIdAsync(string profileId, CancellationToken cancellationToken = default);

	/// <summary>
	/// دریافت علاقمندی توسط شناسه آگهی و کاربر
	/// </summary>
	/// <param name="announcementId">شناسه آگهی</param>
	/// <param name="profileId">شناسه کاربر</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<Favorite?> GetByAnnouncementAndProfileAsync(string announcementId, string profileId, CancellationToken cancellationToken = default);

	/// <summary>
	/// دریافت تعداد علاقمندی های یک آگهی
	/// </summary>
	/// <param name="announcementId">شناسه آگهی</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<int> GetCountByAnnouncementIdAsync(string announcementId, CancellationToken cancellationToken = default);

	/// <summary>
	/// دریافت تمام علاقمندی های یک آگهی
	/// </summary>
	/// <param name="announcementId">شناسه آگهی</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<List<Favorite>> GetByAnnouncementIdAsync(string announcementId, CancellationToken cancellationToken = default);

	/// <summary>
	/// بررسی لیست آیدی های آگهی - آیدی آنهایی که آگهی دارند برمیگردد
	/// </summary>
	/// <param name="ids"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<List<string>> CheckAnnouncementIdsAsync(List<string> ids, string profileId, CancellationToken cancellationToken = default);
	Task<List<string>> GetIdsByProfileIdAsync(string? profileId, CancellationToken cancellationToken = default);

	/// <summary>
	/// حذف دیتای این جدول برای یک کاربر در هنگام حذف حساب کاربری
	/// </summary>
	/// <param name="profileId"></param>
	/// <param name="cancellationToken"></param>
	Task DeleteAccountAsync(
		string profileId,
		CancellationToken cancellationToken = default);
}
