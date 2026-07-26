using FluentResults;
using DynamicFields.Configs;
using DynamicFields.Abstraction;
using ESH.ViewModels.Abstraction;

namespace DynamicFields.Validator;

public class TextFieldValidator : IFieldValidator
{
	public async Task<Result> Validate(object? value, IFieldTypeConfig config)
	{
		var textConfig = (TextConfig)config;

		if (value is not null)
		{
			if (value.ToString()!.Length > textConfig.MaxLength)
			{
				var errorMessage = string.Format(
					ESH.Resources.Messages.StringLengthExceedsTheMaximumLengthErrorMessage,
					textConfig.MaxLength);

				var result =
					Result.Fail(errorMessage);

				return result;
			}
		}

		return Result.Ok();
	}
}