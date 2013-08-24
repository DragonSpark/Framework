using DotNetOpenAuth.AspNet;

namespace DragonSpark.Server.Security
{
	public interface IAuthenticationResultProcessor
	{
		bool Process( AuthenticationResult authenticationResult );
	}
}