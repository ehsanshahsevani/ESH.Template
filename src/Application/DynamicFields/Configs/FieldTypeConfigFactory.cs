using DynamicFields.Abstraction;
using Domain.Constants;
using ESH.ViewModels.Abstraction;

namespace DynamicFields.Configs;

public static class FieldTypeConfigFactory
{
	public static IFieldTypeConfig GetConfig(string fieldType)
	{
		return fieldType switch
		{
			FieldTypes.Int => new NumberConfig { Min = 1, Max = 99999 },
			FieldTypes.Decimal => new NumberConfig { Min = 0, FancyDetection = true },
			FieldTypes.String => new StringConfig { MaxLength = 200 },
			FieldTypes.Text => new TextConfig { MaxLength = 4000 },
			FieldTypes.MultiValue => new MultiValueConfig { UseMultiValues = true },
			FieldTypes.Attachment => new AttachmentConfig
			{
				MaxCount = 5,
				MaxSizeMB = 10,
				AllowedExtensions = ["jpg", "jpeg", "png", "webp"]
			},
			FieldTypes.Location => new LocationConfig { AllowMap = true, AddressSummary = true },
			_ => throw new ArgumentException("Invalid field type")
		};
	}
}