using System.Security.Claims;
using System.Security.Principal;
using DragonSpark.Extensions;
using DragonSpark.Server.Legacy.Security.Security;

namespace DragonSpark.Server.Legacy.Security
{
	public static class PrincipalExtensions
	{
		public static string GetId( this IPrincipal target )
		{
			var result = target.AsTo<ClaimsPrincipal, string>( x => string.Format( "{0}-{1}", x.FindFirst( Claims.IdentityProvider ).Transform( y => y.Value ), x.FindFirst( ClaimTypes.NameIdentifier ).Transform( y => y.Value ) ) );
			return result;
		}
	}
}