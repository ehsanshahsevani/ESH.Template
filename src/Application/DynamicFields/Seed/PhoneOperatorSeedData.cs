using DynamicFields.Models;
using ESH.SeedworkSystem.ViewModel.Base;

namespace DynamicFields.Seed;

public class PhoneOperatorSeedData : ISeedData<PhoneOperatorItem>
{
	private static string SetFileName(string code)
	{
		return $"{nameof(Domain.PhoneOperator)}-{code}.png";
	}

	private static readonly PhoneOperatorItem[] _data =
	[
		new()
		{
			Code = "OMANTEL",
			NameEn = "Omantel",
			NameAr = "عمانتل",
			ImageNameOnAttachmentServer = SetFileName(code: "OMANTEL"),
			Prefixes = ["901", "902"],
			Ordering = 10
		},
		new()
		{
			Code = "OOREDOO",
			NameEn = "Ooredoo",
			NameAr = "أوريدو",
			ImageNameOnAttachmentServer = SetFileName(code: "OOREDOO"),
			Prefixes = ["903", "904"],
			Ordering = 20
		},
		new()
		{
			Code = "VODAFONE",
			NameEn = "Vodafone",
			NameAr = "فودافون",
			ImageNameOnAttachmentServer = SetFileName(code: "VODAFONE"),
			Prefixes = ["906"],
			Ordering = 30
		},
		new()
		{
			Code = "RENNA",
			NameEn = "Renna",
			NameAr = "رنة",
			ImageNameOnAttachmentServer = SetFileName(code: "RENNA"),
			Prefixes = [],
			Ordering = 40
		},
		new()
		{
			Code = "FRIENDI",
			NameEn = "Friendi",
			NameAr = "فريندي",
			ImageNameOnAttachmentServer = SetFileName(code: "FRIENDI"),
			Prefixes = [],
			Ordering = 50
		},
		new()
		{
			Code = "RED_BULL",
			NameEn = "RedBull",
			NameAr = "ريدبُل",
			ImageNameOnAttachmentServer = SetFileName(code: "RED_BULL"),
			Prefixes = [],
			Ordering = 60
		}
	];

	public IReadOnlyList<PhoneOperatorItem> Data => _data;
}