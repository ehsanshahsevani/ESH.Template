using ESH.Utilities;
using ESH.Enums.Shared;

namespace DynamicFields.Seed;

public static class StatusColorSelector
{
	public static string? Get(int code)
	{
		switch (code)
		{
			case 10:
				{
					var color = BootstrapColor.Warning.GetEnumDisplayName();
					return color;
				}
			case 20:
				{
					var color = BootstrapColor.Warning.GetEnumDisplayName();
					return color;
				}
			case 30:
				{
					var color = BootstrapColor.Primary.GetEnumDisplayName();
					return color;
				}
			case 40:
				{
					var color = BootstrapColor.Danger.GetEnumDisplayName();
					return color;
				}
			case 50:
				{
					var color = BootstrapColor.Secondary.GetEnumDisplayName();
					return color;
				}
			default:
				{
					var color = BootstrapColor.None.GetEnumDisplayName();
					return color;
				}
		}
	}
}
