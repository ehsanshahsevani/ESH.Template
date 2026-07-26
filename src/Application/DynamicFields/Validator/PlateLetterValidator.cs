using FluentResults;
using DynamicFields.Abstraction;
using ESH.ViewModels.Abstraction;
using Persistence;

namespace DynamicFields.Validator;

public class PlateLetterValidator : IFieldValidator
{
	private IUnitOfWork UnitOfWork { get; }

	public PlateLetterValidator(IUnitOfWork unitOfWork)
	{
		UnitOfWork = unitOfWork;
	}

	public async Task<Result> Validate(object? value, IFieldTypeConfig config)
	{
		var result = new FluentResults.Result();

		if (value is null)
		{
			result.WithError(ESH.Resources.Messages.ValueCannotBeNull);
			return result;
		}

		var id = value.ToString();

		if (string.IsNullOrWhiteSpace(id) == true)
		{
			result.WithError(ESH.Resources.Messages.ValueCannotBeNull);
			return result;
		}

		var plate = await UnitOfWork
			.PlateCodeRepository.FindAsync(id);

		if (plate is null)
		{
			var errorMessage = string.Format(
				ESH.Resources.Messages.NotFoundError,
				ESH.Resources.DataDictionary.PlateCode);

			result.WithError(errorMessage);

			return result;
		}

		return result;
	}
}
