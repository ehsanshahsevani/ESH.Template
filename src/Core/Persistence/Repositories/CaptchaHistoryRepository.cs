using Domain;
using Persistence.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class CaptchaHistoryRepository
	: Repository<CaptchaHistory>, ICaptchaHistoryRepository
{
	internal CaptchaHistoryRepository(DatabaseContext databaseContext) : base(databaseContext)
	{
	}

	public async Task<CaptchaHistory?> FindByIpAsync(string ip, CancellationToken cancellationToken = default)
	{
		var result = await DbSet

			.Where(current => current.Ip == ip)
			.OrderByDescending(current => current.CreateDateTime)
			.FirstOrDefaultAsync(cancellationToken);

		return result;
	}
}