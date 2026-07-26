using Domain;
using ESH.SeedworkSystem.Persistence;

namespace Persistence.Abstracts;

// For FieldType
public interface IFieldTypeRepository : IRepository<FieldType>
{
	Task<FieldType?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
	Task<List<FieldType>> GetByCodesAsync(List<string> codes, CancellationToken cancellationToken = default);
}
