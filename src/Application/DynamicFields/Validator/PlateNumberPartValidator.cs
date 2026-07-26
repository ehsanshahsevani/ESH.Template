using FluentResults;
using DynamicFields.Abstraction;
using DynamicFields.Configs;
using ESH.ViewModels.Abstraction;

namespace DynamicFields.Validator;

public class PlateNumberPartValidator : IFieldValidator
{
	public async Task<Result> Validate(object? value, IFieldTypeConfig config)
	{
		var result =
			new FluentResults.Result();

		var numberConfig =
			config as NumberConfig;

		if (value is null)
		{
			result.WithError(ESH.Resources.Messages.ValueCannotBeNull);

			return result;
		}

		if (value is int)
		{
			return result;
		}

		if (int.TryParse(value.ToString(), out var p) == false)
		{
			result.WithError(ESH.Resources.Messages.ValueCannotBeNull);
			return result;
		}

		if (p < numberConfig!.Min || p > numberConfig.Max)
		{
			var errorMessage = string.Format(
				ESH.Resources.Messages.MinAndMaxValueFieldError,
				ESH.Resources.DataDictionary.PlateNumber,
				numberConfig.Min,
				numberConfig.Max);

			result.WithError(errorMessage);
		}

		return result;
	}
}
