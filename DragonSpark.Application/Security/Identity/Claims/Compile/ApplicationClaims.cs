using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Claims.Compile
{
	sealed class ApplicationClaims : ClaimExtractor
	{
		public static ApplicationClaims Default { get; } = new ApplicationClaims();

		ApplicationClaims() : base(new[] { ClaimTypes.AuthenticationMethod, "amr" }) {}
	}
}