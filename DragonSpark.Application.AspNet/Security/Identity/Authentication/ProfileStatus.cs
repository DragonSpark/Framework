namespace DragonSpark.Application.AspNet.Security.Identity.Authentication;

public enum ProfileStatus
{
	Invalid,
	Anonymous,
	Authenticated,
	Confirming,
	Confirmed,
	InputRequired
}