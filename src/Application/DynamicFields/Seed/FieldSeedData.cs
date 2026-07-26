using Domain.Constants;
using DynamicFields.Models;
using DynamicFields.Configs;
using ESH.Constant.Announcement;
using ESH.SeedworkSystem.ViewModel.Base;

namespace DynamicFields.Seed;

public sealed class FieldPlateSeedData : ISeedData<FieldSeedModel>
{
	private static readonly FieldSeedModel[] _data =
	[
		new(
			Code: FieldTypes.PlateNumberPart,
			TitleAr: "الجزء الرقمي للوحة",
			TitleEn: "Plate Number",
			HintAr: "الجزء الرقمي للوحة",
			HintEn: "Plate Number",
			DescriptionAr: "الجزء الرقمي للوحة",
			DescriptionEn: "Plate Number",
			DataType: FieldTypes.Int,
			JsonConfig: SeedJson.Of(config: new NumberConfig
			{
				Min = 1,
				Max = 99999,
				FancyDetection = true
			})
		),

		new(
			Code: FieldTypes.PlateLetter,
			TitleAr: "حرف اللوحة",
			TitleEn: "Plate Letter",
			HintAr: "حرف اللوحة",
			HintEn: "Plate Letter",
			DescriptionAr: "حرف اللوحة",
			DescriptionEn: "Plate Letter",
			DataType: FieldTypes.PlateLetter,
			JsonConfig: SeedJson.Of(config: new MultiValueConfig
			{
				UseMultiValues = false,
				IsHiddenCharacters = true,
				ReplaceCharactersWhenIsHidden = "**"
			})
		),

		new(
			Code: FieldTypes.Price,
			TitleAr: "سعر",
			TitleEn: "Price",
			HintAr: "سعر",
			HintEn: "Price",
			DescriptionAr: "سعر",
			DescriptionEn: "Price",
			DataType: FieldTypes.Int,
			JsonConfig: SeedJson.Of(config:
				new NumberConfig()
				{
					Min = 0,
					Max = 10000000,
					FancyDetection = false
				})
		),

		new(
			Code: FieldTypes.PlateStatus,
			TitleAr: "حالة اللوحة",
			TitleEn: "Plate Status",
			DataType: FieldTypes.PlateStatus,
			HintAr: "حالة اللوحة",
			HintEn: "Plate Status",
			DescriptionAr: "حالة اللوحة",
			DescriptionEn: "Plate Status",
			JsonConfig: SeedJson.Of(config:
				new MultiValueConfig()
				{
					ShowOneRow = true,
				})
		),

		new(
			Code: FieldTypes.Attachment,
			TitleAr: "صورة",
			TitleEn: "Image",
			DataType: FieldTypes.Attachment,
			HintAr: "صورة",
			HintEn: "Image",
			DescriptionAr: "صورة",
			DescriptionEn: "Image",
			JsonConfig: SeedJson.Of(config: new AttachmentConfig
			{
				MaxCount = 5,
				MaxSizeMB = 10,
				AllowedExtensions = ["jpg", "jpeg", "png", "webp"]
			})
		),

		new(
			Code: FieldTypes.Title,
			TitleAr: "العنوان",
			TitleEn: "Title",
			HintAr: "العنوان",
			HintEn: "Title",
			DescriptionAr: "العنوان",
			DescriptionEn: "Title",
			DataType: FieldTypes.String,
			JsonConfig: SeedJson.Of(config: new StringConfig { MaxLength = 200 })
		),

		new(
			Code: FieldTypes.Region,
			TitleAr: "المنطقة",
			TitleEn: "Region",
			HintAr: "المنطقة",
			HintEn: "Region",
			DescriptionAr: "المنطقة",
			DescriptionEn: "Region",
			DataType: FieldTypes.Region,
			JsonConfig: SeedJson.Of(config: new MultiValueConfig())
		),
	];

	public IReadOnlyList<FieldSeedModel> Data => _data;
}

public sealed class FieldPhoneSeedData : ISeedData<FieldSeedModel>
{
	private static readonly FieldSeedModel[] _data =
	[
		new(
			Code: FieldTypes.PhoneBody,
			TitleAr: "رقم الهاتف",
			TitleEn: "Phone Number",
			HintAr: "رقم الهاتف",
			HintEn: "Phone Number",
			DescriptionAr: "رقم الهاتف",
			DescriptionEn: "Phone Number",
			DataType: FieldTypes.String,
			JsonConfig: SeedJson.Of(config: new StringConfig
			{
				Length = 8,
				Regex = @"^[9][0-9]{7}$"
			})
		),

		new(
			Code: FieldTypes.PhoneOperator,
			TitleAr: "شركة الاتصالات",
			TitleEn: "Mobile Operator",
			HintAr: "شركة الاتصالات",
			HintEn: "Mobile Operator",
			DescriptionAr: "شركة الاتصالات",
			DescriptionEn: "Mobile Operator",
			DataType: FieldTypes.PhoneOperator,
			JsonConfig: SeedJson.Of(config: new MultiValueConfig())
		),
		new(
			Code: FieldTypes.Price,
			TitleAr: "السعر",
			TitleEn: "Price",
			HintAr: "السعر",
			HintEn: "Price",
			DescriptionAr: "السعر",
			DescriptionEn: "Price",
			DataType: FieldTypes.Int,
			JsonConfig: SeedJson.Of(config:
				new NumberConfig()
				{
					Min = 0,
					Max = 10000000,
					FancyDetection = false
				})
		),
	];

	public IReadOnlyList<FieldSeedModel> Data => _data;
}

public sealed class FieldPropertySeedData : ISeedData<FieldSeedModel>
{
	private static readonly FieldSeedModel[] _data =
	[
		new(
			Code: FieldTypes.Attachment,
			TitleAr: "صورة",
			TitleEn: "Image",
			HintAr: "صورة",
			HintEn: "Image",
			DescriptionAr: "صورة",
			DescriptionEn: "Image",
			DataType: FieldTypes.Attachment,
			JsonConfig: SeedJson.Of(config: new AttachmentConfig
			{
				MaxCount = 5,
				MaxSizeMB = 10,
				AllowedExtensions = AttachmentExtensions.Images,
			})
		),

		new(
			Code: FieldTypes.Title,
			TitleAr: "العنوان",
			TitleEn: "Title",
			HintAr: "العنوان",
			HintEn: "Title",
			DescriptionAr: "العنوان",
			DescriptionEn: "Title",
			DataType: FieldTypes.String,
			JsonConfig: SeedJson.Of(config: new StringConfig { MaxLength = 200 })
		),

		new(
			Code: FieldTypes.Price,
			TitleAr: "السعر",
			TitleEn: "Price",
			HintAr: "السعر",
			HintEn: "Price",
			DescriptionAr: "السعر",
			DescriptionEn: "Price",
			DataType: FieldTypes.Int,
			JsonConfig: SeedJson.Of(config:
				new NumberConfig()
				{
					Min = 0,
					Max = 10000000,
					FancyDetection = false
				})
		),

		new(
			Code: FieldTypes.Location,
			TitleAr: "الموقع",
			TitleEn: "Location",
			HintAr: "الموقع",
			HintEn: "Location",
			DescriptionAr: "الموقع",
			DescriptionEn: "Location",
			DataType: FieldTypes.Location,
			JsonConfig: SeedJson.Of(config: new LocationConfig
			{
				AllowMap = true,
				AddressSummary = false
			})
		),

		new(
			Code: FieldTypes.Region,
			TitleAr: "المنطقة",
			TitleEn: "Region",
			HintAr: "المنطقة",
			HintEn: "Region",
			DescriptionAr: "المنطقة",
			DescriptionEn: "Region",
			DataType: FieldTypes.Region,
			JsonConfig: SeedJson.Of(config: new MultiValueConfig())
		),

		new(
			Code: FieldTypes.Description,
			TitleAr: "الوصف",
			TitleEn: "Description",
			HintAr: "الوصف",
			HintEn: "Description",
			DescriptionAr: "الوصف",
			DescriptionEn: "Description",
			DataType: FieldTypes.Text,
			JsonConfig: SeedJson.Of(config: new TextConfig
			{
				MaxLength = 4000
			})
		)
	];

	public IReadOnlyList<FieldSeedModel> Data => _data;
}

public sealed class FieldOtherSeedData : ISeedData<FieldSeedModel>
{
	private static readonly FieldSeedModel[] _data =
	[
		new(
			Code: FieldTypes.Attachment,
			TitleAr: "صورة",
			TitleEn: "Image",
			HintAr: "صورة",
			HintEn: "Image",
			DescriptionAr: "صورة",
			DescriptionEn: "Image",
			DataType: FieldTypes.Attachment,
			JsonConfig: SeedJson.Of(config: new AttachmentConfig
			{
				MaxCount = 5,
				MaxSizeMB = 10,
				AllowedExtensions = ["jpg", "jpeg", "png", "webp"]
			})
		),
		new(
			Code: FieldTypes.Title,
			TitleAr: "العنوان",
			TitleEn: "Title",
			HintAr: "العنوان",
			HintEn: "Title",
			DescriptionAr: "العنوان",
			DescriptionEn: "Title",
			DataType: FieldTypes.String,
			JsonConfig: SeedJson.Of(config: new StringConfig { MaxLength = 200 })
		),
		new(
			Code: FieldTypes.Price,
			TitleAr: "السعر",
			TitleEn: "Price",
			HintAr: "السعر",
			HintEn: "Price",
			DescriptionAr: "السعر",
			DescriptionEn: "Price",
			DataType: FieldTypes.Int,
			JsonConfig: SeedJson.Of(config:
				new NumberConfig()
				{
					Min = 0,
					Max = 10000000,
					FancyDetection = false
				})
		),
		new(
			Code: FieldTypes.Price,
			TitleAr: "السعر",
			TitleEn: "Price",
			HintAr: "السعر",
			HintEn: "Price",
			DescriptionAr: "السعر",
			DescriptionEn: "Price",
			DataType: FieldTypes.Int,
			JsonConfig: SeedJson.Of(config:
				new NumberConfig()
				{
					Min = 0,
					Max = 10000000,
					FancyDetection = false
				})
		),
		// Code: FieldTypes.CustomValues is field type
		new(
			Code: FieldTypes.CustomValues,
			TitleAr: "",
			TitleEn: "",
			HintAr: "",
			HintEn: "",
			DescriptionAr: "",
			DescriptionEn: "",
			DataType: FieldTypes.CustomValues,
			JsonConfig: SeedJson.Of(config: new MultiValueConfig())
		),
	];

	public IReadOnlyList<FieldSeedModel> Data => _data;
}