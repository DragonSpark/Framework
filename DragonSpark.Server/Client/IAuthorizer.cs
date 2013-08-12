using System.Security.Principal;

namespace DragonSpark.Server.Client
{
	public interface IAuthorizer
	{
		bool IsAuthorized( IPrincipal principal );
	}
}