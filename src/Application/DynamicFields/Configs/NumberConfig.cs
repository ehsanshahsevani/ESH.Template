using DynamicFields.Abstraction;
using ESH.ViewModels.Abstraction;

namespace DynamicFields.Configs;

public sealed class NumberConfig : IFieldTypeConfig
{
	public int? Min { get; init; }
	public int? Max { get; init; }
	public bool FancyDetection { get; init; }
}