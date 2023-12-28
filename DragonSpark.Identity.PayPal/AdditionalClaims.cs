using DragonSpark.Application.Security.Identity.Claims.Compile;

namespace DragonSpark.Identity.PayPal;

sealed class AdditionalClaims : Application.Security.Identity.Claims.Compile.AdditionalClaims
{
	public AdditionalClaims(IKnownClaims previous) : base(previous, KnownClaims.Default) {}
}