using DynamicFields.Configs;
using System.Text.Json;

namespace Repositories;

public class ProfileRepositoryTest : Base.BaseTestWithDatabaseInMemory
{
	#region Database_Constructor_InitialDat

	public ProfileRepositoryTest() : base()
	{
	}

	#endregion /Database_Constructor_InitialData

	[Fact]
	public void LocationTest()
	{
		var location =
			new Location
			{
				Latitude = 12,
				Longitude = 16,
				AddressSummary = "IRAN, Shiraz"
			};


		var stringLocation = JsonSerializer.Serialize<Location>(location);
		var locationValue = JsonSerializer.Deserialize<Location>(stringLocation);
	}
}