using System.Text.Json.Serialization;
using ESH.BuildingBlocks.RequestFeatures;

namespace ViewModels.Parameters;

public class BaseRequestParameter : RequestParameters
{
	[JsonIgnore]
	public virtual bool? IsActive { get; set; }
}

public class CategoryParameters : BaseRequestParameter;
