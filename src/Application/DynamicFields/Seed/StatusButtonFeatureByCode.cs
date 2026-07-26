using ESH.ViewModels.Announcement;

namespace DynamicFields.Seed;

public static class StatusButtonFeatureByCode
{
	public static ButtonFeature Get(int code)
	{
		switch (code)
		{
			case 10:
				{
					var result =
						new ButtonFeature(
							hidden: true,
							delete: true,
							update: false);

					return result;
				}
			case 20:
				{
					var result =
						new ButtonFeature(
							hidden: false,
							delete: true,
							update: true);

					return result;
				}
			case 30:
				{
					var result =
						new ButtonFeature(
							hidden: true,
							delete: true,
							update: true);

					return result;
				}
			case 40:
			case 50:
				{
					var result =
						new ButtonFeature(
							hidden: false,
							delete: false,
							update: false);

					return result;
				}
			default:
				{
					var result =
						new ButtonFeature(
							hidden: false,
							delete: false,
							update: false);

					return result;
				}
		}
	}
}