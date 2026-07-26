using DynamicFields.Abstraction;
using ESH.ViewModels.Abstraction;

namespace DynamicFields.Configs;

public sealed class StaticMultiConfig<TItems> : IFieldTypeConfig
{
	public bool Localized { get; init; }
	public TItems[] Values { get; init; }
}