using ESH.SeedworkSystem.ViewModel.Base;

namespace Infrastructure.BuildingBlocks.LookupSeeds.Status;

public sealed class StatusSeedData :
	ISeedData<StatusSeedModel>
{
	private static readonly StatusSeedModel[] _data =
	[
		new(Code: 10,ArOm: "قيد المراجعة",EnUs: "Pending Approval"),
		new(Code: 20,ArOm: "بحاجة إلى تعديل",EnUs: "Needs Modification"),
		new(Code: 30,ArOm: "تم النشر",EnUs: "Published"),
		new(Code: 40,ArOm: "مرفوض",EnUs: "Rejected"),
		new(Code: 50,ArOm: "منتهي",EnUs: "Expired"),
	];

	public IReadOnlyList<StatusSeedModel> Data => _data;
}