using ESH.Constant;
using Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace Domain;

/// <summary>
/// نگهداری کد افراد برای کد امنیتی
/// </summary>
public class CaptchaHistory : BaseEntity
{
	private CaptchaHistory()
	{
	}

	public CaptchaHistory(string ip, int code)
	{
		Ip = ip;
		Code = code;
	}

	// **************************************************
	/// <summary>
	/// آدرس آی پی
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.IpAddress))]
	
	[Required(
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.RequiredError))]
	
	[MaxLength(
		length: MaxLength.IP,
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.MaxLengthError))]

	public string Ip { get; set; }
	// **************************************************

	// **************************************************
	/// <summary>
	/// کد - شش رقمی می باشد
	/// </summary>

	[Display(
		ResourceType = typeof(ESH.Resources.DataDictionary),
		Name = nameof(ESH.Resources.DataDictionary.Code))]
	
	[Required(
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.RequiredError))]

	[MaxLength(
		length: MaxLength.OtpCode,
		ErrorMessageResourceType = typeof(ESH.Resources.Messages),
		ErrorMessageResourceName = nameof(ESH.Resources.Messages.MaxLengthError))]

	public int Code { get; set; }
	// **************************************************
}