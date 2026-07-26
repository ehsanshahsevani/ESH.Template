using Domain;
using Persistence.Abstracts;

namespace Persistence.Repositories;

public class PlateStatusRepository : Repository<PlateStatus>, IPlateStatusRepository
{
	internal PlateStatusRepository(DatabaseContext databaseContext) : base(databaseContext)
	{
	}
}