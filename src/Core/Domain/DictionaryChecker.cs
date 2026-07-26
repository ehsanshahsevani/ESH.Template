using ESH.Constant;
using System.ComponentModel.DataAnnotations;

namespace Domain;

/// <summary>
/// متون خاص و خارج از عرف
/// شامل الفاظ رکیک مربوط به هر زبان
/// </summary>
public class DictionaryChecker : Base.BaseEntity
{
	public DictionaryChecker() : base()
	{
		Announcements = new List<Announcement>();
	}

	// *********************************************
	/// <summary>
	/// متن / محتوا
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.Text))]

	[Required(
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.RequiredError))]

	[MaxLength(
		length: MaxLength.Description,
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.MaxLengthError))]

	public string Text { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// لیست آگهی های مربوط به این وضعیت
	/// </summary>
	public List<Announcement> Announcements { get; set; }
	// *********************************************
}