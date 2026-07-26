using DynamicFields.Abstraction;
using ESH.ViewModels.Abstraction;

namespace DynamicFields.Configs;

public sealed class TextConfig : IFieldTypeConfig
{
	public int MaxLength { get; init; }
}