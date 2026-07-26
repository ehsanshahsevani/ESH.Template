namespace Infrastructure.BuildingBlocks.LookupSeeds.PlateStatus;

public sealed record PlateStatusSeedModel(
	string Code,
	string ArOm,
	string EnUs,
	bool IsDefault
);