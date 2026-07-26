using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence.Abstracts;

namespace Persistence.Repositories;

public class FieldTypeRepository : Repository<FieldType>, IFieldTypeRepository
{
	internal FieldTypeRepository(DatabaseContext databaseContext) : base(databaseContext)
	{
	}

	public override async Task<IEnumerable<FieldType?>>
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

	public async Task<List<FieldType>> GetByCodesAsync(
		List<string> codes,
		CancellationToken cancellationToken = default)
	{
		var result = await DbSet

			.Where(current => current.IsActive == true)
			.Where(current => current.IsDeleted == false)

			.Where(current => codes.Contains(current.Code))

			.OrderBy(current => current.Ordering)
			.ThenByDescending(current => current.CreateDateTime)

			.ToListAsync(cancellationToken);

		return result;
	}

	public async Task<FieldType?> GetByCodeAsync(
		string code,
		CancellationToken cancellationToken = default)
	{
		var result = await DbSet

			.Where(current => current.IsActive == true)
			.Where(current => current.IsDeleted == false)

			.Where(current => current.Code == code)

			.FirstOrDefaultAsync(cancellationToken);

		return result;
	}
}
