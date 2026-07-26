using System.ComponentModel.DataAnnotations;
using ESH.Constant;

namespace Domain;

public class ContactUs : Base.BaseEntity
{
	public ContactUs() : base()
	{
	}
	
	// *********************************************
	/// <summary>
	/// نام
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.FirstName))]
	
	[MaxLength(
		length: MaxLength.Name,
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.MaxLengthError))]

	public string? FirstName { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// نام خانوادگی
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.LastName))]

	[MaxLength(
		length: MaxLength.Name,
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.MaxLengthError))]

	public string? LastName { get; set; }
	// *********************************************
	
	// *********************************************
	/// <summary>
	/// ایمیل
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.EmailAddress))]
	
	[MaxLength(
		length: MaxLength.EmailAddress,
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.MaxLengthError))]

	public string? EmailAddress { get; set; }
	// *********************************************
	
	// *********************************************
	/// <summary>
	/// شماره تلفن
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.PhoneNumber))]

	public string? PhoneNumber { get; set; }
	// *********************************************
}