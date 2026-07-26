namespace Infrastructure.BuildingBlocks.LookupSeeds.CommonReason;

/// <summary>
/// مدل عمومی برای دلایل مختلف
/// </summary>
public sealed record CommonReasonSeedModel(
	int Code,
	string ArOm,
	string EnUs,
	ReasonType Type,
	bool HasDescription = false
);