using FluentResults;
using DynamicFields.Abstraction;
using System.Globalization;
using ESH.ViewModels.Abstraction;

namespace DynamicFields.Validator;

public class PlatePriceValidator : IFieldValidator
{
	public async Task<Result> Validate(object value, IFieldTypeConfig config)
	{
		var result = new FluentResults.Result();

		if (value is null)
		{
			result.WithError(ESH.Resources.Messages.ValueCannotBeNull);
			return result;
		}

		if (value is decimal)
		{
			return result;
		}

		if (value is int)
		{
			return result;
		}

		var price = value.ToString();

		if (string.IsNullOrWhiteSpace(price) == true)
		{
			result.WithError(ESH.Resources.Messages.ValueCannotBeNull);
			return result;
		}

		if (decimal.TryParse(
				price,
				style: NumberStyles.Number,
				provider: CultureInfo.InvariantCulture, out var _) == false)
		{
			result.WithError(ESH.Resources.Messages.InvalidDecimalFormat);
			return result;
		}

		return result;
	}
}
