using ESH.SeedworkSystem.ViewModel.Base;

namespace Infrastructure.BuildingBlocks.LookupSeeds.PlateCode;

public sealed class PlateCodeSeedData :
	ISeedData<PlateCodeSeedModel>
{
	private static readonly PlateCodeSeedModel[] _data =
	[
        // --- A ---
        new(TypeCode: PlateCodeTypes.Private,ArOm: "ا",EnUs: "A"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "اا",EnUs: "AA"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "اب",EnUs: "AB"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "اد",EnUs: "AD"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "ام",EnUs: "AM"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "ار",EnUs: "AR"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "اس",EnUs: "AS"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "او",EnUs: "AW"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "اي",EnUs: "AY"),

        // --- B ---
        new(TypeCode: PlateCodeTypes.Private,ArOm: "ب",EnUs: "B"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "با",EnUs: "BA"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "بب",EnUs: "BB"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "بد",EnUs: "BD"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "بح",EnUs: "BH"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "بر",EnUs: "BR"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "بس",EnUs: "BS"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "بو",EnUs: "BW"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "بي",EnUs: "BY"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "بم",EnUs: "BM"),

        // --- D ---
        new(TypeCode: PlateCodeTypes.Private,ArOm: "د",EnUs: "D"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "دا",EnUs: "DA"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "دد",EnUs: "DD"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "در",EnUs: "DR"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "دو",EnUs: "DW"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "دي",EnUs: "DY"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "دك",EnUs: "DK"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "دس",EnUs: "DS"),

        // --- H ---
        new(TypeCode: PlateCodeTypes.Private,ArOm: "ح",EnUs: "H"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "حد",EnUs: "HD"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "حح",EnUs: "HH"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "حر",EnUs: "HR"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "حس",EnUs: "HS"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "حي",EnUs: "HY"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "حا",EnUs: "HA"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "حك",EnUs: "HK"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "حم",EnUs: "HM"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "حو",EnUs: "HW"),

        // --- K ---
        new(TypeCode: PlateCodeTypes.Private,ArOm: "ك",EnUs: "K"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "كا",EnUs: "KA"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "كب",EnUs: "KB"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "كح",EnUs: "KH"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "كك",EnUs: "KK"),

        // --- L ---
        new(TypeCode: PlateCodeTypes.Private,ArOm: "لا",EnUs: "LA"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "لب",EnUs: "LB"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "لد",EnUs: "LD"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "لح",EnUs: "LH"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "لك",EnUs: "LK"),

        // --- M ---
        new(TypeCode: PlateCodeTypes.Private,ArOm: "م",EnUs: "M"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "ما",EnUs: "MA"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "مم",EnUs: "MM"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "مو",EnUs: "MW"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "مي",EnUs: "MY"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "مب",EnUs: "MB"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "مد",EnUs: "MD"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "مح",EnUs: "MH"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "مك",EnUs: "MK"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "مل",EnUs: "ML"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "مر",EnUs: "MR"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "مس",EnUs: "MS"),

        // --- R ---
        new(TypeCode: PlateCodeTypes.Private,ArOm: "ر",EnUs: "R"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "را",EnUs: "RA"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "رم",EnUs: "RM"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "رر",EnUs: "RR"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "رس",EnUs: "RS"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "رو",EnUs: "RW"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "ري",EnUs: "RY"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "رح",EnUs: "RH"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "رك",EnUs: "RK"),

        // --- S ---
        new(TypeCode: PlateCodeTypes.Private,ArOm: "س",EnUs: "S"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "سس",EnUs: "SS"),

        // --- T ---
        new(TypeCode: PlateCodeTypes.Private,ArOm: "ط",EnUs: "T"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "طا",EnUs: "TA"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "طب",EnUs: "TB"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "طد",EnUs: "TD"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "طح",EnUs: "TH"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "طط",EnUs: "TT"),

        // --- W ---
        new(TypeCode: PlateCodeTypes.Private,ArOm: "و",EnUs: "W"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "وا",EnUs: "WA"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "وب",EnUs: "WB"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "وك",EnUs: "WK"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "وو",EnUs: "WW"),

        // --- Y ---
        new(TypeCode: PlateCodeTypes.Private,ArOm: "ي",EnUs: "Y"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "يا",EnUs: "YA"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "يي",EnUs: "YY"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "يك",EnUs: "YK"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "يد",EnUs: "YD"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "يم",EnUs: "YM"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "يب",EnUs: "YP"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "يس",EnUs: "YS"),
		new(TypeCode: PlateCodeTypes.Private,ArOm: "يو",EnUs: "YW"),

		new(TypeCode: PlateCodeTypes.Commercial,ArOm: "ا",EnUs: "A"),
		new(TypeCode: PlateCodeTypes.Commercial,ArOm: "ب",EnUs: "B"),
		new(TypeCode: PlateCodeTypes.Commercial,ArOm: "د",EnUs: "D"),
		new(TypeCode: PlateCodeTypes.Commercial,ArOm: "ح",EnUs: "H"),
		new(TypeCode: PlateCodeTypes.Commercial,ArOm: "ك",EnUs: "K"),
		new(TypeCode: PlateCodeTypes.Commercial,ArOm: "ل",EnUs: "L"),
		new(TypeCode: PlateCodeTypes.Commercial,ArOm: "م",EnUs: "M"),
		new(TypeCode: PlateCodeTypes.Commercial,ArOm: "ر",EnUs: "R"),
		new(TypeCode: PlateCodeTypes.Commercial,ArOm: "س",EnUs: "S"),
		new(TypeCode: PlateCodeTypes.Commercial,ArOm: "ط",EnUs: "T"),
		new(TypeCode: PlateCodeTypes.Commercial,ArOm: "و",EnUs: "W"),
		new(TypeCode: PlateCodeTypes.Commercial,ArOm: "ي",EnUs: "Y"),
	];

	public IReadOnlyList<PlateCodeSeedModel> Data => _data;
}