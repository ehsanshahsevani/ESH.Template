using FluentResults;
using DynamicFields.Configs;
using DynamicFields.Abstraction;
using ESH.ViewModels.Abstraction;

namespace DynamicFields.Validator;

public class MultiValueFieldValidator : IFieldValidator
{
	public async Task<Result> Validate(object? value, IFieldTypeConfig config)
	{
		var multiValueConfig = (MultiValueConfig)config;

		if (value is null)
		{
			var errorMessage =
				ESH.Resources.Messages.ValueCannotBeNull;

			var result =
				Result.Fail(errorMessage);

			return result;
		}

		if (multiValueConfig.UseMultiValues)
		{
			if (value is not IEnumerable<object> values || values.Any() == false)
			{
				var errorMessage =
					ESH.Resources.Messages.MultipleValuesAreRequired;

				var result = Result.Fail(errorMessage);

				return result;
			}
		}
		else
		{
			if (value is IEnumerable<object> values && values.Any() == true)
			{
				var errorMessage =
					ESH.Resources.Messages.OnlyOneValueAllowedErrorMessage;

				var result = Result.Fail(errorMessage);

				return result;
			}
		}

		return Result.Ok();
	}
}