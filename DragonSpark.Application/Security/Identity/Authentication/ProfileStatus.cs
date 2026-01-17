namespace DragonSpark.Application.Security.Identity.Authentication;

public enum ProfileStatus : byte
{
	Invalid,
	Anonymous,
	Authenticated,
	Confirming,
	Confirmed,
	InputRequired
}