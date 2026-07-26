using System.Text.Json;
using DynamicFields.Abstraction;
using ESH.ViewModels.Abstraction;

namespace DynamicFields.Configs;

public static class SeedJson
{
	private static readonly JsonSerializerOptions _options = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase
	};

	public static string Of(IFieldTypeConfig config)
	{
		var result =
			JsonSerializer.Serialize(
				config,
				config.GetType(),
				_options
			);

		return result;
	}
}