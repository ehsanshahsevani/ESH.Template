namespace Infrastructure.BuildingBlocks.LookupSeeds.Category;

/// <summary>
/// مدل سید برای دسته‌بندی
/// Category Seed Model
/// </summary>
public sealed record CategorySeedModel(
	string Code,
	string NameAr,
	string NameEn,
	string CategoryTypeCode,

	string FileNameLarge,
	string FileNameSmall,

	int Ordering = 0,
	string? ParentCode = null,
	List<CategorySeedModel>? Children = null
);