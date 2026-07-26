using ESH.SeedworkSystem.ViewModel.Base;

namespace Infrastructure.BuildingBlocks.LookupSeeds.CommonReason;

public sealed class CommonReasonSeedData : ISeedData<CommonReasonSeedModel>
{
	private static readonly CommonReasonSeedModel[] data =
	[
		// -------------------- دلایل گزارش آگهی --------------------
		new(Code: 10, ArOm: "محتوى احتيالي أو خادع", EnUs: "Fraudulent or misleading content", Type: ReasonType.Report), // محتوای فریبنده یا جعلی
		new(Code: 20, ArOm: "محتوى غير لائق أو مسيء", EnUs: "Inappropriate or offensive content", Type: ReasonType.Report), // محتوای نامناسب یا توهین‌آمیز
		new(Code: 30, ArOm: "انتهاك حقوق الملكية", EnUs: "Violation of intellectual property", Type: ReasonType.Report), // نقض حقوق مالکیت
		new(Code: 40, ArOm: "تكرار أو بريد مزعج", EnUs: "Duplicate or spam", Type: ReasonType.Report), // تکراری یا اسپم
		new(Code: 50, ArOm: "معلومات غير دقيقة أو خاطئة", EnUs: "Incorrect or false information", Type: ReasonType.Report), // اطلاعات نادرست یا اشتباه
		new(Code: 60, ArOm: "سلوك غير قانوني أو خطير", EnUs: "Illegal or dangerous behavior", Type: ReasonType.Report), // رفتار غیرقانونی یا خطرناک
		new(Code: 99, ArOm: "أخرى (يمكن للمستخدم إضافة شرح)", EnUs: "Other (user can provide explanation)", Type: ReasonType.Report, HasDescription: true), // سایر

		// -------------------- دلایل نیاز به ویرایش --------------------
		new(Code: 10, ArOm: "معلومات ناقصة", EnUs: "Incomplete information", Type: ReasonType.Edit), // اطلاعات ناقص
		new(Code: 20, ArOm: "صورة أو ملف ناقص", EnUs: "Incomplete image or file", Type: ReasonType.Edit), // تصویر یا فایل ناقص
		new(Code: 30, ArOm: "مشكلة في تنسيق المحتوى", EnUs: "Content format issue", Type: ReasonType.Edit), // مشکل فرمت محتوا
		new(Code: 99, ArOm: "أخرى (يمكن للمستخدم إضافة شرح)", EnUs: "Other (user can provide explanation)", Type: ReasonType.Edit, HasDescription: true), // سایر

		// -------------------- دلایل حذف توسط کاربر --------------------
		new(Code: 10, ArOm: "الإعلان لم يعد صالحًا", EnUs: "Ad is no longer valid", Type: ReasonType.UserDeleted), // آگهی دیگر معتبر نیست
		new(Code: 20, ArOm: "رغبة في عدم العرض للعامة", EnUs: "Want to hide publicly", Type: ReasonType.UserDeleted), // تمایل به عدم نمایش عمومی
		new(Code: 99, ArOm: "أخرى (يمكن للمستخدم إضافة شرح)", EnUs: "Other (user can provide explanation)", Type: ReasonType.UserDeleted, HasDescription: true) // سایر
	];

	public IReadOnlyList<CommonReasonSeedModel> Data => data;
}