using System.ComponentModel.DataAnnotations;

namespace Domain;

/// <summary>
/// دلیل حذف آگهی
/// </summary>
public class DeleteReason : Base.BaseEntity
{
	#region Constatnts

	public const string TextPropertyLocalizer = "Text";

	#endregion /Constrants

	public DeleteReason(int code) : base()
	{
		Code = code;
		Announcements = new List<Announcement>();
	}

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
	/// لیست آگهی های مربوط به این وضعیت
	/// </summary>
	public List<Announcement> Announcements { get; set; }
	// *********************************************
}