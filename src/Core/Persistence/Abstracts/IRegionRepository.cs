using Domain;
using ESH.SeedworkSystem.Persistence;

namespace Persistence.Abstracts;

public interface IRegionRepository : IRepository<Region>
{
	Task<List<Region?>>
		GetByParentIdAsync(string parentId, CancellationToken cancellationToken = default);
}