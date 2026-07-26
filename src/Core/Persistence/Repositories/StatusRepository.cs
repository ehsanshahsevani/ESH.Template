using Domain;
using ESH.ViewModels.Shared;
using Persistence.Abstracts;
using ESH.ViewModels.Announcement;
using Microsoft.EntityFrameworkCore;
  

namespace Persistence.Repositories;

public class StatusRepository : Repository<Status>, IStatusRepository
{
	internal StatusRepository(DatabaseContext databaseContext) : base(databaseContext)
	{
	}

	/// <summary>
	/// جستجو براساس کد
	/// </summary>
	/// <param name="code"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<Status?> FindByCodeAsync(
		int code, CancellationToken cancellationToken = default)
	{
		var result = await DbSet

			.Where(current => current.IsDeleted == false)
			.Where(current => current.IsActive == true)

			.Where(current => current.Code == code)

			.OrderBy(current => current.Ordering)
			.ThenByDescending(current => current.CreateDateTime)

			.FirstOrDefaultAsync(cancellationToken);

		return result;
	}

	/// <summary>
	/// دریافت پک دیتای کامل دیتا با تعداد کل
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<List<CounterDataPack<Status>>>
		GetCounterDataPackAsync(CancellationToken cancellationToken = default)
	{
		var result = await DbSet

			.Include(navigationPropertyPath: current => current.Announcements)

			.Where(current => current.IsDeleted == false)
			.Where(current => current.IsActive == true)

			.OrderBy(current => current.Ordering)
			.ThenByDescending(current => current.CreateDateTime)

			.Select(current => new CounterDataPack<Status>
			{
				Data = current,
				CountAll = current.Announcements.Count(x => x.IsDeleted == false),

				UnreadCount = 0,
			})

			.ToListAsync(cancellationToken);

		return result;
	}
	
	/// <summary>
	/// دریافت دیتای اپ
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<List<CounterDataPack<Status>>>
		GetListAppDataAsync(CancellationToken cancellationToken = default)
	{
		var result = await DbSet
				
			.Where(current => current.IsDeleted == false)
			.Where(current => current.IsActive == true)

			.OrderBy(current => current.Ordering)
			.ThenByDescending(current => current.CreateDateTime)

			.Select(current => new CounterDataPack<Status>
			{
				Data = current,

				CountAll = 0,
				UnreadCount = 0,
			})

			.ToListAsync(cancellationToken);

		return result;
	}

	/// <summary>
	/// دریافت دیتای مربوط به پارت های فرانت
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<List<ChartDataViewModel>>
		GetChartDataForStatusAsync(CancellationToken cancellationToken = default)
	{
		var result = await DbSet

			.Where(current => current.IsDeleted == false)

			.OrderBy(current  => current.Ordering)
			.ThenByDescending(current => current.CreateDateTime)

			.Select(current =>
				new ChartDataViewModel(
					current.Id,
					string.Empty,
					current.Announcements.Count)
				)

			.ToListAsync(cancellationToken);

		return result;
	}
}
