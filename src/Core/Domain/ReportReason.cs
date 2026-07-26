using System.ComponentModel.DataAnnotations;

namespace Domain;

/// <summary>
/// دلایل گزارش کردن یک آگهی در صورتی که تخلف کرده است
/// </summary>
public class ReportReason : Base.BaseEntity
{
	#region Constatnts

	public const string TextPropertyLocalizer = "Text";

	#endregion /Constrants

	public ReportReason(int code) : base()
	{
		Code = code;
		ReportLogs = new List<ReportLog>();
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
	/// لیست گزارشات با این دلیل
	/// </summary>
	public List<ReportLog> ReportLogs { get; set; }
	// *********************************************
}