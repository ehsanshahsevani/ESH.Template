using ESH.SeedworkSystem.ViewModel.Base;

namespace Infrastructure.BuildingBlocks.LookupSeeds.PlateStatus;

public class PlateStatusSeedData : ISeedData<PlateStatusSeedModel>
{
	private static readonly PlateStatusSeedModel[] _data =
	[
		new(
			Code: "PRIVATE",
			ArOm: "خصوصي",
			EnUs: "private",
			IsDefault: true
		),
		new(
			Code: "COMMERCIAL",
			ArOm: "تجاري",
			EnUs: "commercial",
			IsDefault: false
		),
	];

	public IReadOnlyList<PlateStatusSeedModel> Data => _data;
}