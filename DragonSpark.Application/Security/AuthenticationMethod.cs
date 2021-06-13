using System.Security.Claims;

namespace DragonSpark.Application.Security
{
	public sealed class AuthenticationMethod : ReadClaim
	{
		public static AuthenticationMethod Default { get; } = new AuthenticationMethod();

		AuthenticationMethod() : base(ClaimTypes.AuthenticationMethod) {}
	}
}