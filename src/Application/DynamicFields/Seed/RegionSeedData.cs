using DynamicFields.Models;
using ESH.SeedworkSystem.ViewModel.Base;

namespace DynamicFields.Seed;

public class RegionSeedData : ISeedData<RegionSeedModel>
{
	private static readonly RegionSeedModel[] _data =
	[
		new(
			Code: "OMAN",
			NameEn: "Oman",
			NameAr: "عُمان",
			ParentCode: null,
			Children:
			[
				new(
					Code: "MUSCAT",
					NameEn: "Muscat",
					NameAr: "مسقط",
					ParentCode: "OMAN",
					Children:
					[
						new(Code: "MUSCAT", NameEn: "Muscat", NameAr: "مسقط", ParentCode: "MUSCAT", Children: []),
						new(Code: "MUTTRAH", NameEn: "Muttrah", NameAr: "مطرح", ParentCode: "MUSCAT", Children: []),
						new(Code: "BAWSHAR", NameEn: "Bawshar", NameAr: "بوشر", ParentCode: "MUSCAT", Children: []),
						new(Code: "SEEB", NameEn: "Seeb", NameAr: "السيب", ParentCode: "MUSCAT", Children: []),
						new(Code: "ALAMARAT", NameEn: "AlAmarat", NameAr: "العامرات", ParentCode: "MUSCAT", Children: []),
						new(Code: "QURAYYAT", NameEn: "Qurayyat", NameAr: "قريات", ParentCode: "MUSCAT", Children: [])
					]
				),
				new(
					Code: "DHOFAR",
					NameEn: "Dhofar",
					NameAr: "ظفار",
					ParentCode: "OMAN",
					Children:
					[
						new(Code: "SALALAH", NameEn: "Salalah", NameAr: "صلالة", ParentCode: "DHOFAR", Children: []),
						new(Code: "TAQAH", NameEn: "Taqah", NameAr: "طاقة", ParentCode: "DHOFAR", Children: []),
						new(Code: "MIRBAT", NameEn: "Mirbat", NameAr: "مرباط", ParentCode: "DHOFAR", Children: []),
						new(Code: "RAKHYUT", NameEn: "Rakhyut", NameAr: "رخيوت", ParentCode: "DHOFAR", Children: []),
						new(Code: "THUMRAIT", NameEn: "Thumrait", NameAr: "ثمريت", ParentCode: "DHOFAR", Children: []),
						new(Code: "DHALKUT", NameEn: "Dhalkut", NameAr: "ضلكوت", ParentCode: "DHOFAR", Children: []),
						new(Code: "ALMAZYUNAH", NameEn: "AlMazyunah", NameAr: "المزيونة", ParentCode: "DHOFAR", Children: []),
						new(Code: "MUQSHIN", NameEn: "Muqshin", NameAr: "مقشن", ParentCode: "DHOFAR", Children: []),
						new(Code: "SHALIMHALLANIYAT", NameEn: "ShalimHallaniyat", NameAr: "شليم وجزر الحلانيات", ParentCode: "DHOFAR",
							Children: []),
						new(Code: "SADAH", NameEn: "Sadah", NameAr: "سدح", ParentCode: "DHOFAR", Children: [])
					]
				),

				new(
					Code: "MUSANDAM",
					NameEn: "Musandam",
					NameAr: "مسندم",
					ParentCode: "OMAN",
					Children:
					[
						new(Code: "KHASAB", NameEn: "Khasab", NameAr: "خصب", ParentCode: "MUSANDAM", Children: []),
						new(Code: "BUKHA", NameEn: "Bukha", NameAr: "بخاء", ParentCode: "MUSANDAM", Children: []),
						new(Code: "DIBBA", NameEn: "Dibba", NameAr: "دبا", ParentCode: "MUSANDAM", Children: []),
						new(Code: "MADHA", NameEn: "Madha", NameAr: "مدحاء", ParentCode: "MUSANDAM", Children: [])
					]
				),

				new(
					Code: "ALBURAIMI",
					NameEn: "AlBuraimi",
					NameAr: "البريمي",
					ParentCode: "OMAN",
					Children:
					[
						new RegionSeedModel(Code: "ALBURAIMI", NameEn: "AlBuraimi", NameAr: "البريمي", ParentCode: "ALBURAIMI",
							Children: []),
						new RegionSeedModel(Code: "MAHDHA", NameEn: "Mahdha", NameAr: "محضة", ParentCode: "ALBURAIMI", Children: []),
						new RegionSeedModel(Code: "ASSUNAYNAH", NameEn: "AsSunaynah", NameAr: "السنينة", ParentCode: "ALBURAIMI",
							Children: [])
					]
				),

				new(
					Code: "ADDAKHILIYAH",
					NameEn: "AdDaliyah",
					NameAr: "الداخلية",
					ParentCode: "OMAN",
					Children:
					[
						new RegionSeedModel(Code: "NIZWA", NameEn: "Nizwa", NameAr: "نزوى", ParentCode: "ADDAKHILIYAH", Children: []),
						new RegionSeedModel(Code: "BAHLA", NameEn: "Bahla", NameAr: "بهلاء", ParentCode: "ADDAKHILIYAH", Children: []),
						new RegionSeedModel(Code: "MANAH", NameEn: "Manah", NameAr: "منح", ParentCode: "ADDAKHILIYAH", Children: []),
						new RegionSeedModel(Code: "ALHAMRA", NameEn: "AlHamra", NameAr: "الحمراء", ParentCode: "ADDAKHILIYAH",
							Children: []),
						new RegionSeedModel(Code: "ADAM", NameEn: "Adam", NameAr: "آدم", ParentCode: "ADDAKHILIYAH", Children: []),
						new RegionSeedModel(Code: "IZKI", NameEn: "Izki", NameAr: "إزكي", ParentCode: "ADDAKHILIYAH", Children: []),
						new RegionSeedModel(Code: "SAMAIL", NameEn: "Samail", NameAr: "سمائل", ParentCode: "ADDAKHILIYAH", Children: []),
						new RegionSeedModel(Code: "BIDBID", NameEn: "Bidbid", NameAr: "بدبد", ParentCode: "ADDAKHILIYAH", Children: []),
						new RegionSeedModel(Code: "JEBELAKHDAR", NameEn: "JebelAkhdar", NameAr: "الجبل الأخضر", ParentCode: "ADDAKHILIYAH",
							Children: [])
					]
				),

				new(
					Code: "ADDHAHIRAH",
					NameEn: "AdDhahirah",
					NameAr: "الظاهرة",
					ParentCode: "OMAN",
					Children:
					[
						new RegionSeedModel(Code: "IBRI", NameEn: "Ibri", NameAr: "عبري", ParentCode: "ADDHAHIRAH", Children: []),
						new RegionSeedModel(Code: "YANQUL", NameEn: "Yanqul", NameAr: "ينقل", ParentCode: "ADDHAHIRAH", Children: []),
						new RegionSeedModel(Code: "DHANK", NameEn: "Dhank", NameAr: "ضنك", ParentCode: "ADDHAHIRAH", Children: [])
					]
				),


				new(
					Code: "ALBATINAHNORTH",
					NameEn: "AlBatinahNorth",
					NameAr: "شمال الباطنة",
					ParentCode: "OMAN",
					Children:
					[
						new RegionSeedModel(Code: "SOHAR", NameEn: "Sohar", NameAr: "صحار", ParentCode: "ALBATINAHNORTH", Children: []),
						new RegionSeedModel(Code: "SHINAS", NameEn: "Shinas", NameAr: "شناص", ParentCode: "ALBATINAHNORTH", Children: []),
						new RegionSeedModel(Code: "LIWA", NameEn: "Liwa", NameAr: "لوى", ParentCode: "ALBATINAHNORTH", Children: []),
						new RegionSeedModel(Code: "SAHAM", NameEn: "Saham", NameAr: "صحم", ParentCode: "ALBATINAHNORTH", Children: []),
						new RegionSeedModel(Code: "ALKHABOURAH", NameEn: "AlKhabourah", NameAr: "الخابورة", ParentCode: "ALBATINAHNORTH",
							Children: []),
						new RegionSeedModel(Code: "ASSUWAIQ", NameEn: "AsSuwaiq", NameAr: "السويق", ParentCode: "ALBATINAHNORTH",
							Children: [])
					]
				),

				new(
					Code: "ALBATINAHSOUTH",
					NameEn: "AlBatinahSouth",
					NameAr: "جنوب الباطنة",
					ParentCode: "OMAN",
					Children:
					[
						new RegionSeedModel(Code: "RUSTAQ", NameEn: "Rustaq", NameAr: "الرستاق", ParentCode: "ALBATINAHSOUTH",
							Children: []),
						new RegionSeedModel(Code: "ALAWABI", NameEn: "AlAwabi", NameAr: "العوابي", ParentCode: "ALBATINAHSOUTH",
							Children: []),
						new RegionSeedModel(Code: "NAKHAL", NameEn: "Nakhal", NameAr: "نخل", ParentCode: "ALBATINAHSOUTH", Children: []),
						new RegionSeedModel(Code: "WADIALMAAWIL", NameEn: "WadiAlMaawil", NameAr: "وادي المعاول", ParentCode: "ALBATINAHSOUTH",
							Children: []),
						new RegionSeedModel(Code: "BARKA", NameEn: "Barka", NameAr: "بركاء", ParentCode: "ALBATINAHSOUTH", Children: []),
						new RegionSeedModel(Code: "ALMUSANNAH", NameEn: "AlMusannah", NameAr: "المصنعة", ParentCode: "ALBATINAHSOUTH",
							Children: [])
					]
				),

				new(
					Code: "ASHSHARQIYAHSOUTH",
					NameEn: "AshSharqiyahSouth",
					NameAr: "جنوب الشرقية",
					ParentCode: "OMAN",
					Children:
					[
						new RegionSeedModel(Code: "SUR", NameEn: "Sur", NameAr: "صور", ParentCode: "ASHSHARQIYAHSOUTH", Children: []),
						new RegionSeedModel(Code: "ALKAMILWALWAFI", NameEn: "AlKamilWalWafi", NameAr: "الكامل والوافي", ParentCode: "ASHSHARQIYAHSOUTH",
							Children: []),
						new RegionSeedModel(Code: "JALANBANIBUHASSAN", NameEn: "JalanBaniBuHassan", NameAr: "جعلان بني بو حسن",
							ParentCode: "ASHSHARQIYAHSOUTH", Children: []),
						new RegionSeedModel(Code: "JALANBANIBUALI", NameEn: "JalanBaniBuAli", NameAr: "جعلان بني بو علي", ParentCode: "ASHSHARQIYAHSOUTH",
							Children: []),
						new RegionSeedModel(Code: "MASIRAH", NameEn: "Masirah", NameAr: "مصيرة", ParentCode: "ASHSHARQIYAHSOUTH",
							Children: [])
					]
				),

				new(
					Code: "ASHSHARQIYAHNORTH",
					NameEn: "AshSharqiyahNorth",
					NameAr: "شمال الشرقية",
					ParentCode: "OMAN",
					Children:
					[
						new RegionSeedModel(Code: "IBRA", NameEn: "Ibra", NameAr: "إبراء", ParentCode: "ASHSHARQIYAHNORTH", Children: []),
						new RegionSeedModel(Code: "ALMUDHAIBI", NameEn: "AlMudhaibi", NameAr: "المضيبي", ParentCode: "ASHSHARQIYAHNORTH",
							Children: []),
						new RegionSeedModel(Code: "BIDIYA", NameEn: "Bidiya", NameAr: "بدية", ParentCode: "ASHSHARQIYAHNORTH",
							Children: []),
						new RegionSeedModel(Code: "ALQABIL", NameEn: "AlQabil", NameAr: "القابل", ParentCode: "ASHSHARQIYAHNORTH",
							Children: []),
						new RegionSeedModel(Code: "WADIBANIKHALID", NameEn: "WadiBaniKhalid", NameAr: "وادي بني خالد", ParentCode: "ASHSHARQIYAHNORTH",
							Children: []),
						new RegionSeedModel(Code: "DIMAWATTAIYIN", NameEn: "DimaWatTaiyin", NameAr: "دماء والطائيين", ParentCode: "ASHSHARQIYAHNORTH",
							Children: []),
						new RegionSeedModel(Code: "SINAW", NameEn: "Sinaw", NameAr: "سناو", ParentCode: "ASHSHARQIYAHNORTH", Children: [])
					]
				),

				new(
					Code: "ALWUSTA",
					NameEn: "AlWusta",
					NameAr: "الوسطى",
					ParentCode: "OMAN",
					Children:
					[
						new RegionSeedModel(Code: "HAIMA", NameEn: "Haima", NameAr: "هيما", ParentCode: "ALWUSTA", Children: []),
						new RegionSeedModel(Code: "MAHOUT", NameEn: "Mahout", NameAr: "محوت", ParentCode: "ALWUSTA", Children: []),
						new RegionSeedModel(Code: "DUQM", NameEn: "Duqm", NameAr: "الدقم", ParentCode: "ALWUSTA", Children: []),
						new RegionSeedModel(Code: "ALJAZER", NameEn: "AlJazer", NameAr: "الجازر", ParentCode: "ALWUSTA", Children: [])
					]
				)
			]
		)
	];

	public IReadOnlyList<RegionSeedModel> Data => _data;
}