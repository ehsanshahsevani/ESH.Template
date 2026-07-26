
using Domain;
using ESH.SeedworkSystem.Persistence;
using ESH.Utilities;

namespace Persistence.Abstracts;

// For FieldMultiValue
public interface IFieldMultiValueRepository : IRepository<FieldMultiValue>
{
	Task<List<UiSelectModel>> GetByFieldIdAsync(string fieldId, bool? isActive, CancellationToken cancellationToken = default);
}
