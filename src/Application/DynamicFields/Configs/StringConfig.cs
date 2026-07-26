using DynamicFields.Abstraction;
using ESH.ViewModels.Abstraction;

namespace DynamicFields.Configs;

public sealed class StringConfig : IFieldTypeConfig
{
	public int? MaxLength { get; init; }
	public int? Length { get; init; }
	public string? Regex { get; init; }
}