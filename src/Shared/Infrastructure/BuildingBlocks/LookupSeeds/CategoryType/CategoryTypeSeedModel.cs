namespace Infrastructure.BuildingBlocks.LookupSeeds.CategoryType;

/// <summary>
/// مدل برای نوع دسته‌بندی (فقط کد رشته‌ای، بدون چندزبانه)
/// </summary>
public sealed record CategoryTypeSeedModel(string Code, bool HasAccessToChild);