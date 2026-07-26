using DynamicFields.Abstraction;
using ESH.ViewModels.Abstraction;

namespace DynamicFields.Configs;

public sealed class AttachmentConfig : IFieldTypeConfig
{
	public int MaxCount { get; init; }
	public int MaxSizeMB { get; init; }
	public string[] AllowedExtensions { get; init; }
}