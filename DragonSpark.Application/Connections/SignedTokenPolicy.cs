using DragonSpark.Application.Security.Identity.Claims.Policy;

namespace DragonSpark.Application.Connections;

public sealed class SignedTokenPolicy : AddPolicyInstance
{
	public SignedTokenPolicy(IsSigned condition)
		: base(nameof(SignedTokenPolicy), new SignedTokenRequirement(condition)) {}
}