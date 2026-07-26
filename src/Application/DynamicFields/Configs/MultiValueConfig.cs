using DynamicFields.Abstraction;
using ESH.ViewModels.Abstraction;

namespace DynamicFields.Configs;

public sealed class MultiValueConfig : IFieldTypeConfig
{
	/// <summary>
	/// نشان میدهد که این فیلد در یک ردیف نمایش داده شود یا در چند ردیف (هر مقدار در یک ردیف جداگانه)
	/// </summary>
	public bool ShowOneRow { get; set; } = false;

	/// <summary>
	/// قرار است این فیلد چند مقدار داشته باشد یا خیر. اگر این مقدار false باشد، فقط یک مقدار برای این فیلد ذخیره میشود و در صورت ارسال چند مقدار، فقط مقدار اول ذخیره میشود
	/// </summary>
	public bool UseMultiValues { get; init; } = false;

	/// <summary>
	/// کاراکترها مخدوش شوند یا خیر. اگر این مقدار true باشد، کاراکترهایی که قرار است نمایش داده شوند، مخدوش میشوند و به جای آنها کاراکترهایی که در ReplaceCharactersWhenIsHidden مشخص شده است نمایش داده میشود
	/// </summary>
	public bool IsHiddenCharacters { get; set; } = false;

	/// <summary>
	/// کاراکترهایی که هنگام مخدوش بودن نمایش داده میشوند. اگر IsHiddenCharacters برابر true باشد، این کاراکترها نمایش داده میشوند و در غیر این صورت این فیلد نادیده گرفته میشود
	/// </summary>
	public string? ReplaceCharactersWhenIsHidden { get; set; }
}