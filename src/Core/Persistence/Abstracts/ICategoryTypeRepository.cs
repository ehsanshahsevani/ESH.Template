using Domain;
using ESH.SeedworkSystem.Persistence;

namespace Persistence.Abstracts;

// For CategoryType
public interface ICategoryTypeRepository : IRepository<CategoryType>
{
	Task<CategoryType?> FindByCodeAsync(string code, CancellationToken cancellationToken = default);
}
