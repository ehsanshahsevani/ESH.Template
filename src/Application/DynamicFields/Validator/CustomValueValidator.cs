using FluentResults;
using DynamicFields.Abstraction;
using ESH.ViewModels.Abstraction;
using Persistence;

namespace DynamicFields.Validator;

public class CustomValueValidator : IFieldValidator
{
	private IUnitOfWork UnitOfWork { get; }

	public CustomValueValidator(IUnitOfWork unitOfWork)
	{
		UnitOfWork = unitOfWork;
	}
	
	public async Task<Result> Validate(object value, IFieldTypeConfig config)
	{
		var result = new Result();

		return result;
	}
}