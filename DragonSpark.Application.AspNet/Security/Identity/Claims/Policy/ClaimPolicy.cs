namespace DragonSpark.Application.Security.Identity.Claims.Policy;

public class ClaimPolicy : AddPolicyConfiguration
{
	protected ClaimPolicy(string name, string claim) : base(name, new RequireClaim(claim)) {}
}