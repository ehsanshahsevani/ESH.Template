using Domain;
using Persistence.Abstracts;
using Microsoft.EntityFrameworkCore;

using ESH.Utilities;

namespace Persistence.Repositories;

public class FieldMultiValueRepository : Repository<FieldMultiValue>, IFieldMultiValueRepository
{
	internal FieldMultiValueRepository(DatabaseContext databaseContext) : base(databaseContext)
	{
	}

	public async Task<List<UiSelectModel>> GetByFieldIdAsync(
		string fieldId, bool? isActive, CancellationToken cancellationToken = default)
	{
		var result = await DbSet
			.Where(current => current.IsDeleted == false)
			.Where(current => isActive.HasValue == false || current.IsActive == isActive.Value)

			.Where(current => current.FieldId == fieldId)
			
			.Select(current => new UiSelectModel(string.Empty, current.Id))

			.ToListAsync(cancellationToken);

		return result;
	}
}
