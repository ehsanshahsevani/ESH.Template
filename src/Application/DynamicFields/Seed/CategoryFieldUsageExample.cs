using Domain.Constants;
using DynamicFields.Models;

namespace DynamicFields.Seed;

/// <summary>
/// مثال‌هایی از نحوه استفاده از CategoryTypeFieldProvider
/// Examples of how to use CategoryTypeFieldProvider
/// </summary>
public static class CategoryFieldUsageExample
{
	/// <summary>
	/// مثال 1: دریافت تمام فیلدهای مربوط به پلاک
	/// Example 1: Get all fields for Plate category
	/// </summary>
	public static void Example1_GetPlateFields()
	{
		var provider = new CategoryTypeFieldProvider();

		// دریافت فیلدها برای نوع پلاک
		IReadOnlyList<FieldSeedModel> plateFields =
			provider.GetFieldsForCategoryType(categoryType: CategoryTypes.Plate);

		// حالا می‌توانید این فیلدها را برای ثبت یا سایر اهداف استفاده کنید
		foreach (var field in plateFields)
		{
			Console.WriteLine(value: $"Field: {field.Code}, TitleAr: {field.TitleAr}, TitleEn: {field.TitleEn}");
		}
	}

	/// <summary>
	/// مثال 2: دریافت تمام فیلدهای مربوط به شماره تلفن
	/// Example 2: Get all fields for Phone category
	/// </summary>
	public static void Example2_GetPhoneFields()
	{
		var provider = new CategoryTypeFieldProvider();

		IReadOnlyList<FieldSeedModel> phoneFields =
			provider.GetFieldsForCategoryType(categoryType: CategoryTypes.Phone);

		// استفاده از فیلدها
		foreach (var field in phoneFields)
		{
			Console.WriteLine(value: $"Field Code: {field.Code}");
			Console.WriteLine(value: $"Data Type: {field.DataType}");
			Console.WriteLine(value: $"JsonConfig: {field.JsonConfig}");
			Console.WriteLine(value: "---");
		}
	}

	/// <summary>
	/// مثال 3: بررسی اعتبار یک فیلد برای نوع خاص
	/// Example 3: Validate if a field is valid for a category type
	/// </summary>
	public static void Example3_ValidateField()
	{
		var provider = new CategoryTypeFieldProvider();

		// آیا PlateNumberPart برای نوع PLATE معتبر است؟
		bool isValid = provider.ValidateField(categoryType: CategoryTypes.Plate, fieldCode: FieldTypes.PlateNumberPart);
		Console.WriteLine(value: $"Is PlateNumberPart valid for Plate? {isValid}"); // true

		// آیا PhoneBody برای نوع PLATE معتبر است؟
		bool isInvalid = provider.ValidateField(categoryType: CategoryTypes.Plate, fieldCode: FieldTypes.PhoneBody);
		Console.WriteLine(value: $"Is PhoneBody valid for Plate? {isInvalid}"); // false
	}

	/// <summary>
	/// مثال 4: دریافت یک فیلد خاص برای نوع دسته‌بندی
	/// Example 4: Get a specific field for a category type
	/// </summary>
	public static void Example4_GetSpecificField()
	{
		var provider = new CategoryTypeFieldProvider();

		// دریافت فیلد خاص
		var field = provider
			.GetFieldForCategoryType(
				categoryType: CategoryTypes.Plate,
				fieldCode: FieldTypes.Price);

		if (field != null)
		{
			Console.WriteLine(value: $"Field Found: {field.TitleEn} ({field.Code})");
		}
		else
		{
			Console.WriteLine(value: "Field not found or not valid for this category type");
		}
	}

	/// <summary>
	/// مثال 5: دریافت فیلدها برای همه انواع دسته‌بندی
	/// Example 5: Get fields for all category types
	/// </summary>
	public static void Example5_GetAllCategoryFields()
	{
		var provider = new CategoryTypeFieldProvider();

		var allCategoryTypes = new[]
		{
			CategoryTypes.Plate,
			CategoryTypes.Phone,
			CategoryTypes.Property,
			CategoryTypes.Other
		};

		foreach (var categoryType in allCategoryTypes)
		{
			var fields = provider.GetFieldsForCategoryType(categoryType: categoryType);
			Console.WriteLine(value: $"\n=== {categoryType} ===");
			Console.WriteLine(value: $"Total Fields: {fields.Count}");

			foreach (var field in fields)
			{
				Console.WriteLine(value: $"  - {field.TitleEn} ({field.Code})");
			}
		}
	}
}

