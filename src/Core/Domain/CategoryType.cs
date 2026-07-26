using System.ComponentModel.DataAnnotations;
using ESH.Constant;
using Domain.Base;
using Domain.Constants;

namespace Domain;

/// <summary>
/// نوع دسته بندی
/// </summary>
public class CategoryType : BaseEntity
{
	public CategoryType(string code, bool hasAccessToChild = false) : base()
	{
		Code = code;
		HasAccessToChild = hasAccessToChild;
	}

	// *********************************************
	/// <summary>
	/// کد
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.Code))]

	[Required(
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.RequiredError))]

	[MaxLength(
		length: MaxLength.Title,
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.MaxLengthError))]

	public string Code { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// نیاز به بررسی فیلدهایش میباشد یا خیر؟
	/// مثلا نیاز است که فیلدی مثل
	/// ناحیه اول و ناحیه دوم پلاک بررسی شود و به ازای آنها فیلدهای دیگری نیز آنالیز و پر شوند
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.HasNeedToCheckFields))]

	public bool HasNeedToCheckFields { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// نشان میدهد که دسته بندی های مربوط به این نوع دسته بندی
	/// نیاز به یک سری فیلد های از پیش تعیین شده دارد یا ندارد؟
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.HasNeedToDefaultFields))]

	public bool HasNeedToDefaultFields { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// اگر فالس باشد ادمین نمیتواند برای این نوع هیچ دسته بندی بسازد یا حتی به دسته بندی هایی که از این نوع هستند چایلد اضافه کند
	/// </summary>
	public bool HasAccessToChild { get; set; }
	// *********************************************

	// *********************************************
	/// <summary>
	/// دسته بندی های این نوع
	/// </summary>
	public List<Category> Categories { get; set; }
	// *********************************************

	// *********************************************
	public bool IsPlate()
	{
		if (Code == CategoryTypes.Plate)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool IsPhone()
	{
		if (Code == CategoryTypes.Phone)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool IsPhoneOrPlate()
	{
		if (Code == CategoryTypes.Phone
			|| Code == CategoryTypes.Plate
		   )
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
	public bool IsOther()
	{
		if (Code == CategoryTypes.Other)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	// *********************************************
}