using FluentResults;
using DynamicFields.Configs;
using DynamicFields.Abstraction;
using ESH.ViewModels.Abstraction;

namespace DynamicFields.Validator;

public class NumberFieldValidator : IFieldValidator
{
	public async Task<Result> Validate(object? value, IFieldTypeConfig config)
	{
		var numberConfig = (NumberConfig)config;

		if (value is null)
		{
			var errorMessage =
				ESH.Resources.Messages.ValueCannotBeNull;

			var result =
				Result.Fail(errorMessage);

			return result;
		}

		if (value is not int number)
		{
			var errorMessage =
				ESH.Resources.Messages.InvalidNumberFormat;

			var result =
				Result.Fail(errorMessage);

			return result;
		}

		if (numberConfig.Min.HasValue && number < numberConfig.Min)
		{
			var errorMessage = string.Format(
				ESH.Resources.Messages.ValueIsLessMinimumError,
				numberConfig.Min);

			var result = Result.Fail(errorMessage);

			return result;
		}

		if (numberConfig.Max.HasValue && number > numberConfig.Max)
		{
			var errorMessage = string.Format(
				ESH.Resources.Messages.ValueIsGreaterThanTheMaximum,
				numberConfig.Max);

			var result =
				Result.Fail(errorMessage);

			return result;
		}

		return Result.Ok();
	}
}