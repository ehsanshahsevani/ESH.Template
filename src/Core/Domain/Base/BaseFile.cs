namespace Domain.Base;

public static class ServerKeyConstant
{
	public const string Key = "C620E381-9CDE-4A6F-90E3-ACD03D2128BA";
}

public abstract class BaseEntity : ESH.SeedworkSystem.Domain.BaseEntity
{
	public BaseEntity() : base()
	{
		ServerId = ServerKeyConstant.Key;
	}
}

public abstract class BaseProfile : ESH.SeedworkSystem.Domain.BaseProfile
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
	protected BaseProfile() : base()
	{
	}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

	protected BaseProfile(string phoneNumber, string userId) : base(phoneNumber, userId)
	{
		ServerId = ServerKeyConstant.Key;
	}
}