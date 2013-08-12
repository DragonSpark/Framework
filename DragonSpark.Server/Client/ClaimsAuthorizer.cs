using System.Security.Claims;
using System.Security.Principal;
using DragonSpark.Extensions;

namespace DragonSpark.Server.Client
{
	public class ClaimsAuthorizer : IAuthorizer
	{
		public string Type { get; set; }

		public string Value { get; set; }

		public bool IsAuthorized( IPrincipal principal )
		{
			var result = principal.AsTo<ClaimsPrincipal, bool>( x => x.FindFirst( y => y.Type == Type ).Transform( y => Value == null || Value == y.Value ) );
			return result;
		}
	}
}