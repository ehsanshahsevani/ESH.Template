using System.ComponentModel.DataAnnotations;
using ESH.Constant;

namespace Domain;

/// <summary>
/// مقدار فیلدهای یک آگهی که توسط کاربر ثبت شده است
/// </summary>
public class FieldValueAnnouncement : Base.BaseEntity
{
	public FieldValueAnnouncement() : base()
	{
	}

	// *********************************************
	/// <summary>
	/// آگهی
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.Announcement))]

	[Required(
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.RequiredError))]

	[MaxLength(
		length: FixedLength.Guid,
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.MaxLengthError))]

	public string AnnouncementId { get; set; }

	public Announcement? Announcement { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// فیلد مربوطه
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

	// *********************************************
	/// <summary>
	/// مقدار فیلد مورد نظر که توسط کاربران پر شده است
	/// میتواند شامل آیدی از یک جدول دیگر باشد
	/// یا آیدی از مقادیر دلخواه ادمین
	/// یا مقدار متنی
	/// یا حتی مقداری چند تیکه و خاص
	/// مثلا:
	/// (address+lat,lng)
	/// </summary>

	public string? Value { get; set; }
	// *********************************************
}