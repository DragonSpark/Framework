using DragonSpark.Runtime;

namespace DragonSpark.Server.Security
{
	sealed class AuthenticationBaseAddress : EnvironmentVariable
	{
		public static AuthenticationBaseAddress Default { get; } = new AuthenticationBaseAddress();

		AuthenticationBaseAddress() : base(nameof(AuthenticationBaseAddress)) {}
	}
}