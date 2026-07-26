using Domain.Constants;
using ESH.SeedworkSystem.ViewModel.Base;

namespace Infrastructure.BuildingBlocks.LookupSeeds.Category;

/// <summary>
/// سید دیتا برای دسته‌بندی‌های پیش‌فرض
/// Default Categories Seed Data
/// </summary>
public sealed class CategorySeedData : ISeedData<CategorySeedModel>
{
	private static readonly CategorySeedModel[] _data =
	[
        // دسته‌بندی پلاک خودرو
        new(
			Code: CategoryCodes.VEHICLE_PLATES,
			NameAr: "لوحات المركبات",
			NameEn: "Vehicle Plates",
			CategoryTypeCode: CategoryTypes.Plate,

			FileNameLarge: DefaultCategoryFileNames.PlateNumberLarge,
			FileNameSmall: DefaultCategoryFileNames.PlateNumberSmall,

			ParentCode: null,
			Ordering: 10,

			Children: []
		),

        // دسته‌بندی شماره تلفن
        new(
			Code: CategoryCodes.PHONE_NUMBERS,
			NameAr: "أرقام الهواتف",
			NameEn: "Phone Numbers",
			CategoryTypeCode: CategoryTypes.Phone,

			FileNameLarge: DefaultCategoryFileNames.PhoneNumberLarge,
			FileNameSmall: DefaultCategoryFileNames.PhoneNumberSmall,

			ParentCode: null,
			Ordering: 20,

			Children: []
		),
        
        // دسته‌بندی املاک
        new(
			Code: CategoryCodes.PROPERTIES,
			NameAr: "العقارات",
			NameEn: "Properties",
			CategoryTypeCode: CategoryTypes.Property,

			FileNameLarge: DefaultCategoryFileNames.PropertyLarge,
			FileNameSmall: DefaultCategoryFileNames.PropertySmall,

			ParentCode: null,
			Ordering: 30,

			Children: []
		)
	];

	public IReadOnlyList<CategorySeedModel> Data => _data;
}




