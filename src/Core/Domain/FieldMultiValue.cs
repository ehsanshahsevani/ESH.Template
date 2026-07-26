using ESH.Constant;
using System.ComponentModel.DataAnnotations;

namespace Domain;

/// <summary>
/// جدول میانی برای فیلدهایی که کاربر باید از بین چند مقدار یکی را انتخاب کند
/// این فیلدها توسط ادمین ثبت و تغییر میکنند
/// </summary>
public class FieldMultiValue : Base.BaseEntity
{
	public FieldMultiValue() : base()
	{
	}

	#region Constatnts

	public const string TextPropertyLocalizer = "Text";

	#endregion /Constrants

	// *********************************************
	/// <summary>
	/// کلید پایدار برای این مقدار
	/// مثلا:
	/// Private
	/// Public
	/// VipPlate
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.Key))]

	[Required(
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.RequiredError))]

	[MaxLength(
		length: MaxLength.Key,
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.MaxLengthError))]

	public string Key { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// فیلد ایجاد شده توسط ادمین یا تیم فنی
	/// توجه داشته باشید که تایپ مربوط به این فیلد باید از نوعی بنام 
	/// CUSTOME_VALUES
	/// باشد
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.Field))]

	[Required(
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.RequiredError))]

	[MaxLength(
		length: FixedLength.Guid,
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.MaxLengthError))]

	public string FieldId { get; set; }

	public Field? Field { get; set; }
	// *********************************************
}