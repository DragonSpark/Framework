using JetBrains.Annotations;

namespace DragonSpark.Application.Security.Identity.Claims.Policy;

[UsedImplicitly]
public class ClaimsPolicy : AddPolicyConfiguration
{
	protected ClaimsPolicy(string name, params string[] claims) : base(name, new RequireClaims(claims)) {}
}