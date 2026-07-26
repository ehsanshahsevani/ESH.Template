using DynamicFields.Abstraction;
using Persistence;
using Domain.Constants;

namespace DynamicFields.Validator;

public class FieldValidatorFactory
{
	private readonly IUnitOfWork _unitOfWork;

	public FieldValidatorFactory(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;

		if (_unitOfWork == null)
		{
			throw new ArgumentNullException(nameof(unitOfWork));
		}
	}

	public IFieldValidator Resolve(string fieldType)
	{
		switch (fieldType)
		{
			case FieldTypes.Int:
				return new NumberFieldValidator();
			case FieldTypes.Decimal:
				return new DecimalFieldValidator();
			case FieldTypes.String:
				return new StringFieldValidator();
			case FieldTypes.Title:
				return new StringFieldValidator();
			case FieldTypes.Text:
				return new TextFieldValidator();
			case FieldTypes.Description:
				return new TextFieldValidator();
			case FieldTypes.MultiValue:
				return new MultiValueFieldValidator();
			case FieldTypes.Attachment:
				return new AttachmentFieldValidator();
			case FieldTypes.Location:
				return new LocationFieldValidator();
			case FieldTypes.PlateStatus:
				return new PlateStatusValidator(_unitOfWork);
			case FieldTypes.Region:
				return new RegionValidator(_unitOfWork);
			case FieldTypes.PhoneOperator:
				return new PhoneOperatorValidator(_unitOfWork);
			case FieldTypes.PhoneBody:
				return new StringFieldValidator();
			case FieldTypes.PlateNumberPart:
				return new PlateNumberPartValidator();
			case FieldTypes.PlateLetter:
				return new PlateLetterValidator(_unitOfWork);
			case FieldTypes.Price:
				return new PlatePriceValidator();
			case FieldTypes.CustomValues:
				return new CustomValueValidator(_unitOfWork);
			default:
				throw new NotImplementedException(
					$"Field validator for type '{fieldType}' is not implemented.");
		}
	}
}
