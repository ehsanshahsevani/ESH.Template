using ESH.Constant;
using ESH.Resources;
using System.ComponentModel.DataAnnotations;

namespace Domain;

/// <summary>
/// منطقه
/// </summary>
public class Region : Base.BaseEntity
{
	#region Constatnts

	public const string PropertyNameKey = "Name";

	#endregion /Constrants

	private Region() : base()
	{
		Regions = [];
	}

	public Region(string code)
	{
		Code = code;
		Regions = [];
	}

	// *********************************************
	/// <summary>
	/// کد
	/// </summary>

	[Display(
		ResourceType = typeof(DataDictionary),
		Name = nameof(DataDictionary.Code))]

	[Required(
		ErrorMessageResourceType = typeof(Messages),
		ErrorMessageResourceName = nameof(Messages.RequiredError))]

	[MaxLength(
		MaxLength.Title,
		ErrorMessageResourceType = typeof(Messages),
		ErrorMessageResourceName = nameof(Messages.MaxLengthError))]

	public string Code { get; set; }
	// *********************************************

	// **************************************************
	/// <summary>
	/// شناسه والد
	/// </summary>

	[Display(
		ResourceType = typeof(DataDictionary),
		Name = nameof(DataDictionary.Parent))]

	[MaxLength(
		length: FixedLength.Guid,
		ErrorMessageResourceType = typeof(Messages),
		ErrorMessageResourceName = nameof(Messages.MaxLengthError))]

	public string? ParentId { get; set; }

	public Region? Parent { get; set; }
	// **************************************************

	// **************************************************
	/// <summary>
	/// لیست ریجن های زیر مجموعه
	/// </summary>
	public List<Region> Regions { get; set; }
	// **************************************************
}