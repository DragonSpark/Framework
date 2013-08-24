using System.Security.Principal;

namespace DragonSpark.Server.ClientHosting
{
	public interface IAuthorizer
	{
		bool IsAuthorized( IPrincipal principal );
	}
}