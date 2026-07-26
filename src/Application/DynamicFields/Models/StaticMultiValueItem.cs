namespace DynamicFields.Models;

// Item → config نیست
public sealed class StaticMultiValueItem
{
	public string Key { get; init; }
	public int SortOrder { get; init; }
	public string? Icon { get; init; }
}