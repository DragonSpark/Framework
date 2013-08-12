using System.Security.Principal;

namespace DragonSpark.Server.Client
{
	public class AnonymousAuthorizer : IAuthorizer
	{
		public bool IsAuthorized( IPrincipal principal )
		{
			var result = !principal.Identity.IsAuthenticated;
			return result;
		}
	}
}