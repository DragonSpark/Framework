using System.Linq;
using System.Security.Principal;
using DragonSpark.Extensions;

namespace DragonSpark.Server.ClientHosting
{
	public class RolesAuthorizer : IAuthorizer
	{
		public string Roles { get; set; }

		public bool IsAuthorized( IPrincipal principal )
		{
			var result = Roles.ToStringArray().All( principal.IsInRole );
			return result;
		}
	}
}