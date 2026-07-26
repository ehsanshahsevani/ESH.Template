using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence.Abstracts;

using ESH.BuildingBlocks.RequestFeatures;
using ESH.ViewModels.Announcement.ModelParameters;
 

namespace Persistence.Repositories;

public class ReportLogRepository : Repository<ReportLog>, IReportLogRepository
{
	internal ReportLogRepository(DatabaseContext databaseContext) : base(databaseContext)
	{
	}

	/// <summary>
	/// بررسی اینکه آیا یک گزارش قبلا توسط کاربر برای این آگهی ثبت شده است یا نه
	/// </summary>
	/// <param name="announcementId">شناسه آگهی</param>
	/// <param name="profileId">شناسه کاربر</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<bool> ExistsByAnnouncementAndProfileAsync(string announcementId, string profileId, CancellationToken cancellationToken = default)
	{
		var exists = await DbSet
			.Where(x => x.AnnouncementId == announcementId)
			.Where(x => x.ProfileId == profileId)
			.AnyAsync(cancellationToken);

		return exists;
	}

	/// <summary>
	/// دریافت همه به صورت دسته بندی شده
	/// </summary>
	/// <param name="parameter"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<PagedList<ReportLog>> GetAllWithPageAsync(
		ReportLogParameters parameter,
		CancellationToken cancellationToken = default)
	{
		var source = DbSet

			.Include(current => current.Profile)

			.Where(current => current.IsDeleted == false)
			.Where(current => current.IsActive == true)

			.AsQueryable()

			;

		var result =
			await PagedList<ReportLog>.ToPagedList(
					source, parameter, cancellationToken);

		return result;
	}

	public async Task<int> CountAsync(CancellationToken cancellationToken = default)
	{
		var oneWeekAgo = DateTime.UtcNow.AddDays(-30);

		var result = await DbSet
			.Where(current => current.IsDeleted == false)
			.Where(current => current.CreateDateTime >= oneWeekAgo)
			.CountAsync(cancellationToken);

		return result;
	}

	public async Task<List<string>> GetAnnouncementIdsAsync()
	{
		var result = await DbSet
			.Where(current => current.IsDeleted == false)
			.Select(current => current.AnnouncementId)
			.ToListAsync();

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

