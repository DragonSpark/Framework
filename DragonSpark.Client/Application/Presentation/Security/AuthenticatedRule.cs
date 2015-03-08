using System.Security.Principal;
using DragonSpark.Application.Presentation.ComponentModel;

namespace DragonSpark.Application.Presentation.Security
{
	public class AuthenticationContext : ViewObject
	{}

	public class AuthenticatedRule : ISecurityRule
	{
		public bool Check( IPrincipal principal )
		{
			var result = principal.Identity.IsAuthenticated;
			return result;
		}
	}
}