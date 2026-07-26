using Domain.Constants;
using DynamicFields.Seed;

namespace UnitTest.DynamicFields;

/// <summary>
/// تست‌های مربوط به CategoryTypeFieldProvider
/// </summary>
public class CategoryTypeFieldProviderTests
{
	#region GetFieldsForCategoryType_Plate_ShouldReturnCompleteFieldData

	[Fact(DisplayName = "تست دریافت فیلدهای مربوط به پلاک باید داده‌های کامل فیلد را برگرداند")]
	public void GetFieldsForCategoryType_Plate_ShouldReturnCompleteFieldData()
	{
		// Arrange
		var provider = new CategoryTypeFieldProvider();

		// Act
		var fields = provider.GetFieldsForCategoryType(CategoryTypes.Plate);

		// Assert
		Assert.NotNull(fields);
		Assert.NotEmpty(fields);

		// بررسی که تمام فیلدها اطلاعات کامل دارند
		foreach (var field in fields)
		{
			Assert.NotNull(field);
			Assert.NotEmpty(field.Code);
			Assert.NotEmpty(field.TitleAr);
			Assert.NotEmpty(field.TitleEn);
			Assert.NotEmpty(field.DataType);
			Assert.NotEmpty(field.JsonConfig);
		}

		// بررسی وجود فیلدهای خاص پلاک
		Assert.Contains(fields, f => f.Code == FieldTypes.PlateNumberPart);
		Assert.Contains(fields, f => f.Code == FieldTypes.PlateLetter);
		Assert.Contains(fields, f => f.Code == FieldTypes.PlateStatus);
		Assert.Contains(fields, f => f.Code == FieldTypes.Price);
		Assert.Contains(fields, f => f.Code == FieldTypes.Region);
	}

	#endregion

	#region GetFieldsForCategoryType_Phone_ShouldReturnCompleteFieldData

	[Fact(DisplayName = "تست دریافت فیلدهای مربوط به تلفن باید داده‌های کامل فیلد را برگرداند")]
	public void GetFieldsForCategoryType_Phone_ShouldReturnCompleteFieldData()
	{
		// Arrange
		var provider = new CategoryTypeFieldProvider();

		// Act
		var fields = provider.GetFieldsForCategoryType(CategoryTypes.Phone);

		// Assert
		Assert.NotNull(fields);
		Assert.NotEmpty(fields);

		// بررسی وجود فیلدهای خاص تلفن
		Assert.Contains(fields, f => f.Code == FieldTypes.PhoneBody);
		Assert.Contains(fields, f => f.Code == FieldTypes.PhoneOperator);
		Assert.Contains(fields, f => f.Code == FieldTypes.Price);
	}

	#endregion

	#region GetFieldsForCategoryType_Property_ShouldReturnCompleteFieldData

	[Fact(DisplayName = "تست دریافت فیلدهای مربوط به املاک باید داده‌های کامل فیلد را برگرداند")]
	public void GetFieldsForCategoryType_Property_ShouldReturnCompleteFieldData()
	{
		// Arrange
		var provider = new CategoryTypeFieldProvider();

		// Act
		var fields = provider.GetFieldsForCategoryType(CategoryTypes.Property);

		// Assert
		Assert.NotNull(fields);
		Assert.NotEmpty(fields);

		// بررسی وجود فیلدهای خاص املاک
		Assert.Contains(fields, f => f.Code == FieldTypes.Title);
		Assert.Contains(fields, f => f.Code == FieldTypes.Price);
		Assert.Contains(fields, f => f.Code == FieldTypes.Location);
		Assert.Contains(fields, f => f.Code == FieldTypes.Region);
		Assert.Contains(fields, f => f.Code == FieldTypes.Attachment);
	}

	#endregion

	#region GetFieldForCategoryType_ValidField_ShouldReturnField

	[Fact(DisplayName = "دریافت یک فیلد معتبر برای نوع دسته‌بندی باید فیلد را برگرداند")]
	public void GetFieldForCategoryType_ValidField_ShouldReturnField()
	{
		// Arrange
		var provider = new CategoryTypeFieldProvider();

		// Act
		var field = provider.GetFieldForCategoryType(CategoryTypes.Plate, FieldTypes.PlateNumberPart);

		// Assert
		Assert.NotNull(field);
		Assert.Equal(FieldTypes.PlateNumberPart, field.Code);
		Assert.NotEmpty(field.TitleAr);
		Assert.NotEmpty(field.TitleEn);
	}

	#endregion

	#region GetFieldForCategoryType_InvalidField_ShouldReturnNull

	[Fact(DisplayName = "دریافت یک فیلد نامعتبر برای نوع دسته‌بندی باید null برگرداند")]
	public void GetFieldForCategoryType_InvalidField_ShouldReturnNull()
	{
		// Arrange
		var provider = new CategoryTypeFieldProvider();

		// Act
		var field = provider.GetFieldForCategoryType(CategoryTypes.Plate, FieldTypes.PhoneBody);

		// Assert
		Assert.Null(field);
	}

	#endregion

	#region ValidateField_ValidField_ShouldReturnTrue

	[Fact(DisplayName = "بررسی اعتبار فیلد معتبر باید true برگرداند")]
	public void ValidateField_ValidField_ShouldReturnTrue()
	{
		// Arrange
		var provider = new CategoryTypeFieldProvider();

		// Act
		var isValid = provider.ValidateField(CategoryTypes.Plate, FieldTypes.PlateNumberPart);

		// Assert
		Assert.True(isValid);
	}

	#endregion

	#region ValidateField_InvalidField_ShouldReturnFalse

	[Fact(DisplayName = "بررسی اعتبار فیلد نامعتبر باید false برگرداند")]
	public void ValidateField_InvalidField_ShouldReturnFalse()
	{
		// Arrange
		var provider = new CategoryTypeFieldProvider();

		// Act
		var isValid = provider.ValidateField(CategoryTypes.Plate, FieldTypes.PhoneBody);

		// Assert
		Assert.False(isValid);
	}

	#endregion

	#region Constructor_ShouldLoadAllFieldsWithoutDuplicates

	[Fact(DisplayName = "Constructor باید تمام فیلدها را بدون تکرار بارگذاری کند")]
	public void Constructor_ShouldLoadAllFieldsWithoutDuplicates()
	{
		// Arrange & Act
		var provider = new CategoryTypeFieldProvider();

		// Assert - دریافت فیلدها برای تمام انواع
		var plateFields = provider.GetFieldsForCategoryType(CategoryTypes.Plate);
		var phoneFields = provider.GetFieldsForCategoryType(CategoryTypes.Phone);
		var propertyFields = provider.GetFieldsForCategoryType(CategoryTypes.Property);
		var otherFields = provider.GetFieldsForCategoryType(CategoryTypes.Other);

		// بررسی که هر کدام فیلد دارند
		Assert.NotEmpty(plateFields);
		Assert.NotEmpty(phoneFields);
		Assert.NotEmpty(propertyFields);
		Assert.NotEmpty(otherFields);

		// بررسی که فیلدهای مشترک (مثل Price) در همه موجود است
		var priceInPlate = plateFields.Any(f => f.Code == FieldTypes.Price);
		var priceInPhone = phoneFields.Any(f => f.Code == FieldTypes.Price);
		var priceInProperty = propertyFields.Any(f => f.Code == FieldTypes.Price);
		var priceInOther = otherFields.Any(f => f.Code == FieldTypes.Price);

		Assert.True(priceInPlate);
		Assert.True(priceInPhone);
		Assert.True(priceInProperty);
		Assert.True(priceInOther);
	}

	#endregion
}

