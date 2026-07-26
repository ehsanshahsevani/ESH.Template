using Persistence;
using FluentResults;
using DynamicFields.Abstraction;
using ESH.ViewModels.Abstraction;

namespace DynamicFields.Validator;

public class PhoneOperatorValidator : IFieldValidator
{
	private IUnitOfWork UnitOfWork { get; }

	public PhoneOperatorValidator(IUnitOfWork unitOfWork)
	{
		UnitOfWork = unitOfWork;
	}

	public async Task<Result> Validate(object value, IFieldTypeConfig config)
	{
		var result = new Result();

		var id = value.ToString();

		var tryToGuid = Guid.TryParse(id, out _);

		if (tryToGuid == false)
		{
			var errorMessage =
				ESH.Resources.ResponseErrors.RequestNotValid400;

			result.WithError(errorMessage);

			return result;
		}

		var entity = await UnitOfWork
			.PhoneOperatorRepository.FindAsync(id!);

		if (entity is null)
		{
			var errorMessage = string.Format(
				ESH.Resources.Messages.NotFoundError,
				ESH.Resources.DataDictionary.PhoneOperator);

			result.WithError(errorMessage);
		}

		return result;
	}
}
