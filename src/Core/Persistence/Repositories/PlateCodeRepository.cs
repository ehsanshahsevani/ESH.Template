using Domain;
using Persistence.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class PlateCodeRepository : Repository<PlateCode>, IPlateCodeRepository
{
	internal PlateCodeRepository(DatabaseContext databaseContext) : base(databaseContext)
	{
	}

	public override async Task<IEnumerable<PlateCode?>> GetAllAsync(CancellationToken cancellationToken = default)
	{
		var result = await DbSet
			.Where(current => current.IsDeleted == false)
			.OrderBy(current => current.EnUs)
			.ToListAsync(cancellationToken);
		
		return result;
	}
}