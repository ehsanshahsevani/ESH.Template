using Domain.Constants;

namespace DynamicFields.Constants;

public static class FieldTypeUseInFilter
{
	public static readonly string[] Types =
	[
		FieldTypes.Price,
		FieldTypes.Region,

		FieldTypes.PlateLetter,
		FieldTypes.PlateStatus,
		FieldTypes.PlateNumberPart,

		FieldTypes.PhoneBody,
		FieldTypes.PhoneOperator,

		FieldTypes.MultiValue,
		
		FieldTypes.CustomValues,
	];
}