using ESH.SeedworkSystem.ViewModel.Base;

namespace Infrastructure.BuildingBlocks.LookupSeeds.CategoryType;

/// <summary>
/// سید دیتا برای نوع دسته‌بندی
/// فقط شامل کد است
/// </summary>
public sealed class CategoryTypeSeedData : ISeedData<CategoryTypeSeedModel>
{
	private static readonly CategoryTypeSeedModel[] _data =
	[
		new(Code: CategoryTypes.Plate, HasAccessToChild: false),
		new(Code: CategoryTypes.Phone, HasAccessToChild: false),
		new(Code: CategoryTypes.Property, HasAccessToChild: true),
		new(Code: CategoryTypes.Other, HasAccessToChild: true)
	];

	public IReadOnlyList<CategoryTypeSeedModel> Data => _data;
}