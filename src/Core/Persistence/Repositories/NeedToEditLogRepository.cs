using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence.Abstracts;

namespace Persistence.Repositories;

public class NeedToEditLogRepository : Repository<NeedToEditLog>, INeedToEditLogRepository
{
	internal NeedToEditLogRepository(DatabaseContext databaseContext) : base(databaseContext)
	{
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
