using DragonSpark.Runtime;

namespace DragonSpark.Services.Security
{
	sealed class AuthenticationSessionToken : EnvironmentVariable
	{
		public static AuthenticationSessionToken Default { get; } = new AuthenticationSessionToken();

		AuthenticationSessionToken() : base(nameof(AuthenticationSessionToken)) {}
	}
}