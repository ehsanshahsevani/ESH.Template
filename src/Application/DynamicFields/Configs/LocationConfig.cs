using DynamicFields.Abstraction;
using ESH.ViewModels.Abstraction;

namespace DynamicFields.Configs;

public sealed class LocationConfig : IFieldTypeConfig
{
	public bool AllowMap { get; init; }
	public bool AddressSummary { get; init; }
}

public class Location : object
{
	public Location() : base()
	{
	}

	public double Latitude { get; set; }
	public double Longitude { get; set; }
	public string AddressSummary { get; set; }
}
