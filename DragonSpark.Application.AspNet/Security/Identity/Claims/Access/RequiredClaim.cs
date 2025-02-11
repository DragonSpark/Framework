namespace DragonSpark.Application.AspNet.Security.Identity.Claims.Access;

public class RequiredClaim : Application.Security.Identity.Claims.RequiredClaim
{
	protected RequiredClaim(string claim)
		: base(claim,
		       x => $"Content not found for claim '{claim}' in user #'{x.Number()}' {x.Identity?.AuthenticationType}-{x.Identity?.Name}.") {}
}