namespace DynamicFields.Models;

// Operator → config نیست
public sealed class PhoneOperatorItem
{
	public string Code { get; init; }
	public string NameAr { get; init; }
	public string NameEn { get; init; }
	public string ImageNameOnAttachmentServer { get; set; }
	public int Ordering { get; init; }
	public List<string> Prefixes { get; init; }
}