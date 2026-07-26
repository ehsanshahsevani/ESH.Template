using System.ComponentModel.DataAnnotations;
using ESH.Constant;

namespace Domain;

/// <summary>
/// اوپراتور شماره های عمان
/// </summary>
public class PhoneOperator : Base.BaseEntity
{
	#region Constatnts

	public const string NamePropertyLocalizer = "Name";

	#endregion /Constrants

	private PhoneOperator() : base()
	{
	}

	public PhoneOperator(string code) : base()
	{
		Code = code;
	}

	// *********************************************
	/// <summary>
	/// کد
	/// </summary>

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
	// *********************************************

	// *********************************************
	/// <summary>
	/// پیش شماره ها
	/// ممکن است بعضی اوپراتورها هیچ پیش شماره خاصی نداشته باشد
	/// "986,980"
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.Code))]

	[Required(
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.RequiredError))]

	public string? Prefix { get; set; }
	// *********************************************
}