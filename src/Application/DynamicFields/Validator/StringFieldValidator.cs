using FluentResults;
using DynamicFields.Configs;
using DynamicFields.Abstraction;
using ESH.ViewModels.Abstraction;

namespace DynamicFields.Validator;

public class StringFieldValidator : IFieldValidator
{
	public async Task<Result> Validate(object? value, IFieldTypeConfig config)
	{
		var stringConfig = (StringConfig)config;

		if (value is null)
		{
			var errorMessage =
				ESH.Resources.Messages.ValueCannotBeNull;

			var result =
				Result.Fail(errorMessage);

			return result;
		}

		if (value is not string strValue)
		{
			var errorMessage =
				ESH.Resources.Messages.StringInvalidValue;

			var result = Result.Fail(errorMessage);

			return result;
		}

		if (stringConfig.MaxLength.HasValue && strValue.Length > stringConfig.MaxLength)
		{
			var errorMessage = string.Format(
				ESH.Resources.Messages.StringLengthExceedsTheMaximumLengthErrorMessage,
				stringConfig.MaxLength);

			var result =
				Result.Fail(errorMessage);

			return result;
		}

		if (stringConfig.Length.HasValue && strValue.Length != stringConfig.Length)
		{
			var errorMessage = string.Format(
				ESH.Resources.Messages.StringLengthExceedsTheMaximumLengthErrorMessage,
				stringConfig.Length);

			var result =
				Result.Fail(errorMessage);

			return result;
		}

		if (string.IsNullOrWhiteSpace(stringConfig.Regex) == false)
		{
			var regex =
				new System.Text.RegularExpressions.Regex(stringConfig.Regex);

			if (regex.IsMatch(strValue) == false)
			{
				var errorMessage =
					ESH.Resources.Messages.StringInvalidValue;

				var result = Result.Fail(errorMessage);

				return result;
			}
		}

		return Result.Ok();
	}
}