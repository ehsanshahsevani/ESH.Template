namespace DynamicFields.Models;

public sealed record RegionSeedModel(
	string Code,
	string NameEn,
	string NameAr,
	string ParentCode,
	List<RegionSeedModel> Children);