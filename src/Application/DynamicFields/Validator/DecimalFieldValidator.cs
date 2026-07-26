using FluentResults;
using System.Globalization;
using DynamicFields.Configs;
using DynamicFields.Abstraction;
using ESH.ViewModels.Abstraction;

namespace DynamicFields.Validator;

public class DecimalFieldValidator : IFieldValidator
{
	public async Task<Result> Validate(object? value, IFieldTypeConfig config)
	{
		var decimalConfig = (DecimalConfig)config;

		if (value is null)
		{
			var errorMessage =
				ESH.Resources.Messages.ValueCannotBeNull;

			var result = Result.Fail(errorMessage);

			return result;
		}

		if (value is not decimal decimalValue)
		{
			var errorMessage =
				ESH.Resources.Messages.InvalidDecimalFormat;

			var result = Result.Fail(errorMessage);

			return result;
		}

		if (decimalConfig.Min.HasValue && decimalValue < decimalConfig.Min)
		{
			var errorMessage = string.Format(
				ESH.Resources.Messages.ValueIsLessMinimumError,
				decimalConfig.Min);

			var result =
				Result.Fail(errorMessage);

			return result;
		}

		var scale =
			decimalValue
				.ToString(CultureInfo.InvariantCulture)
				.Split('.')[1]
				.Length;

		if (scale > decimalConfig.Scale)
		{
			var errorMessage = string.Format(
				ESH.Resources.Messages.DecimalValueExceedsScaleError,
				decimalConfig.Scale);

			var result =
				Result.Fail(errorMessage);

			return result;
		}

		return Result.Ok();
	}
}