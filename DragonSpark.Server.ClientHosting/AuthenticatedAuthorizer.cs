using System.Security.Principal;

namespace DragonSpark.Server.ClientHosting
{
	public class AuthenticatedAuthorizer : IAuthorizer
	{
		public bool IsAuthorized( IPrincipal principal )
		{
			var result = principal.Identity.IsAuthenticated;
			return result;
		}
	}
}