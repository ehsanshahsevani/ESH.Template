using System.ComponentModel.DataAnnotations;

namespace Domain;

/// <summary>
/// یادداشت خاص و مربوط به کلمات پوشش نداردها
/// </summary>
public class Note : Base.BaseEntity
{
	public Note() : base()
	{
	}

	// *********************************************
	/// <summary>
	/// ریلیشن به جدول Announcement
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.Announcement))]

	[Required(
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.RequiredError))]

	public string AnnouncementId { get; set; }
	public Announcement Announcement { get; set; }

	// *********************************************
	/// <summary>
	/// ریلیشن به جدول Profile
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.Profile))]

	[Required(
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.RequiredError))]

	public string ProfileId { get; set; }
	public Profile Profile { get; set; }

	// *********************************************
	/// <summary>
	/// متنی که به آن نام یادداشت شده است
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.Text))]

	[Required(
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.RequiredError))]

	public string Text { get; set; }
	// *********************************************
}