using System.ComponentModel.DataAnnotations;
using ESH.Constant;
using Domain.Base;

namespace Domain;

/// <summary>
/// وضعیت پلاک (خصوصی / تجاری)
/// </summary>
public class PlateStatus : BaseEntity
{
	// **************************************************
	// خصوصي
	// تجاري

	// commercial
	// private
	// **************************************************

	#region Constants

	public const string PropertyNameKey = "Name";

	#endregion /Constants

	private PlateStatus() { }

	public PlateStatus(string code, bool isDefault) : base()
	{
		Code = code;
		IsDefault = isDefault;
	}

	// **************************************************
	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.Code))]

	[Required(
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.RequiredError))]

	[MaxLength(
		length: MaxLength.Title,
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.MaxLengthError))]

	public string Code { get; set; }
	// **************************************************

	// **************************************************
	/// <summary>
	/// آیا این وضعیت پلاک پیش فرض است یا خیر
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.IsDefault))]

	public bool IsDefault { get; set; }
	// **************************************************
}