using Domain;
using ESH.SeedworkSystem.Persistence;
using ESH.ViewModels.Announcement;
using ESH.ViewModels.Shared;

namespace Persistence.Abstracts;

// For Status
public interface IStatusRepository : IRepository<Status>
{
	/// <summary>
	/// find by code feild value in status
	/// </summary>
	/// <param name="code"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<Status?> FindByCodeAsync(
		int code, CancellationToken cancellationToken = default);
	Task<List<ChartDataViewModel>> GetChartDataForStatusAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// دریافت پک دیتای کامل با تعداد کل
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<List<CounterDataPack<Status>>>
		GetCounterDataPackAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// دریافت دیتای اپ
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<List<CounterDataPack<Status>>>
		GetListAppDataAsync(CancellationToken cancellationToken = default);
}
