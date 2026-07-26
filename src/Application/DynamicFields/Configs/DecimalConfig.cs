using DynamicFields.Abstraction;
using ESH.ViewModels.Abstraction;

namespace DynamicFields.Configs;

public sealed class DecimalConfig : IFieldTypeConfig
{
	public decimal? Min { get; init; }
	public int Scale { get; init; } = 3;
}