using Domain;
using Persistence.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class RegionRepository : Repository<Region>, IRegionRepository
{
	internal RegionRepository(DatabaseContext databaseContext) : base(databaseContext)
	{
	}

	public override async Task<IEnumerable<Region?>>
		GetAllAsync(CancellationToken cancellationToken = default)
	{
		var result = await DbSet
			.Where(current => current.IsDeleted == false)
			.Where(current => current.IsActive == true)

			.OrderBy(current => current.Ordering)
			.ThenByDescending(current => current.CreateDateTime)

			.ToListAsync(cancellationToken);

		return result;
	}

	public async Task<List<Region?>>
		GetByParentIdAsync(string parentId, CancellationToken cancellationToken = default)
	{
		var result = await DbSet
			.Where(current => current.IsDeleted == false)
			.Where(current => current.IsActive == true)

			.Where(current => current.ParentId == parentId)

			.OrderBy(current => current.Ordering)
			.ThenByDescending(current => current.CreateDateTime)

			.ToListAsync(cancellationToken);

		return result;
	}
}