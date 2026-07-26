using Domain;
using ESH.SeedworkSystem.Persistence;

namespace Persistence.Abstracts;

public interface ICaptchaHistoryRepository: IRepository<CaptchaHistory>
{
	Task<CaptchaHistory?> FindByIpAsync(string ip, CancellationToken cancellationToken = default);
}