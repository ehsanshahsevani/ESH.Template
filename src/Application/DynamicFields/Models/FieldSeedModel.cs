namespace DynamicFields.Models;

public sealed record FieldSeedModel(
	string Code,
	string TitleAr,
	string TitleEn,
	string HintAr,
	string HintEn,
	string DescriptionAr,
	string DescriptionEn,
	string DataType,
	string JsonConfig,
	short Version = 1,
	bool IsField = true
);