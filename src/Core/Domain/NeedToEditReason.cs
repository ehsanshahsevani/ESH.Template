using System.ComponentModel.DataAnnotations;

namespace Domain;

/// <summary>
/// دلایل ویرایش آگهی که ادمین برای کاربران انتخاب میکند
/// </summary>
public class NeedToEditReason : Base.BaseEntity
{
	#region Constatnts

	public const string TextPropertyLocalizer = "Text";

	#endregion /Constrants

	public NeedToEditReason(int code) : base()
	{
		Code = code;
		NeedToEditLogs = [];
	}

	// *********************************************
	/// <summary>
	/// کد مربوط به روند کار سیستم
	///  برای بررسی و آپدیت و اعمال مورد چندزبانگی
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.Code))]

	[Required(
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.RequiredError))]

	public int Code { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// نیاز به توضیحات اضافه دارد یا خیر؟
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.HasDescription))]

	public bool HasDescription { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// لیست رکوردهایی که از این دلیل ویرایش استفاده کرده اند
	/// </summary>
	public List<NeedToEditLog> NeedToEditLogs { get; set; }
	// *********************************************
}