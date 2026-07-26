using Domain;
using Persistence.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class FavoriteRepository : Repository<Favorite>, IFavoriteRepository
{
	internal FavoriteRepository(DatabaseContext databaseContext) : base(databaseContext)
	{
	}

	/// <summary>
	/// دریافت تمام علاقمندی های یک کاربر
	/// </summary>
	/// <param name="profileId">شناسه کاربر</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<List<Favorite>> GetByProfileIdAsync(string profileId, CancellationToken cancellationToken = default)
	{
		var result = await DbSet
			.Where(x => x.IsDeleted == false)
			.Where(x => x.ProfileId == profileId)
			.ToListAsync(cancellationToken);

		return result;
	}

	/// <summary>
	/// دریافت علاقمندی توسط شناسه آگهی و کاربر
	/// </summary>
	/// <param name="announcementId">شناسه آگهی</param>
	/// <param name="profileId">شناسه کاربر</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<Favorite?> GetByAnnouncementAndProfileAsync(string announcementId, string profileId, CancellationToken cancellationToken = default)
	{
		var result = await DbSet
			.Where(x => x.IsDeleted == false)
			.Where(x => x.AnnouncementId == announcementId)
			.Where(x => x.ProfileId == profileId)
			.FirstOrDefaultAsync(cancellationToken);

		return result;
	}

	/// <summary>
	/// دریافت تعداد علاقمندی های یک آگهی
	/// </summary>
	/// <param name="announcementId">شناسه آگهی</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<int> GetCountByAnnouncementIdAsync(string announcementId, CancellationToken cancellationToken = default)
	{
		var result = await DbSet
			.Where(x => x.IsDeleted == false)
			.Where(x => x.AnnouncementId == announcementId)
			.CountAsync(cancellationToken);

		return result;
	}

	/// <summary>
	/// دریافت تمام علاقمندی های یک آگهی
	/// </summary>
	/// <param name="announcementId">شناسه آگهی</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<List<Favorite>> GetByAnnouncementIdAsync(string announcementId, CancellationToken cancellationToken = default)
	{
		var result = await DbSet
			.Where(x => x.IsDeleted == false)
			.Where(x => x.AnnouncementId == announcementId)
			.ToListAsync(cancellationToken);

		return result;
	}

	/// <summary>
	/// بررسی لیست آیدی های آگهی - آیدی آنهایی که آگهی دارند برمیگردد
	/// </summary>
	/// <param name="ids"></param>
	/// <param name="profileId"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<List<string>> CheckAnnouncementIdsAsync(List<string> ids, string profileId, CancellationToken cancellationToken = default)
	{
		var existingIds = await DbSet
			.Where(x => x.IsDeleted == false)
			.Where(x => ids.Contains(x.AnnouncementId))
			.Where(x => x.ProfileId == profileId)
			.Select(x => x.AnnouncementId)
			.ToListAsync(cancellationToken);

		return existingIds;
	}

	public async Task<List<string>> GetIdsByProfileIdAsync(
		string? profileId, CancellationToken cancellationToken = default)
	{
		var result = await DbSet

			.Where(current => current.IsDeleted == false)

			.Where(current => current.ProfileId == profileId)

			.Select(current => current.Id)

			.ToListAsync(cancellationToken);

		return result;
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
		await DbSet
			.Where(x => x.Announcement.ProfileId == profileId || x.ProfileId == profileId)
			.ExecuteDeleteAsync(cancellationToken);
	}
	
	#endregion /DeleteAccountAsync(string profileId)
}
