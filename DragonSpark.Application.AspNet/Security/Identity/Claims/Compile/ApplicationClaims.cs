using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Claims.Compile;

sealed class ApplicationClaims : ClaimExtractor
{
	public static ApplicationClaims Default { get; } = new();

	ApplicationClaims() : base(new[] { ClaimTypes.AuthenticationMethod, "amr" }) {}
}