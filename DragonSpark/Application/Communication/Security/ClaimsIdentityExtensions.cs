using DragonSpark.Extensions;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace DragonSpark.Application.Communication.Security
{
    public static class ClaimsIdentityExtensions
    {
		public static string Find( this ClaimsIdentity target, string type )
		{
			var result = target.FindFirst( y => y.Type == type ).Transform( z => z.Value );
			return result;
		}



        public static string DetermineUniqueName( this IIdentity target )
        {
            var result = target.AsTo<ClaimsIdentity,string>( x => string.Concat( x.Find( ClaimTypes.NameIdentifier ), x.Find( Claims.IdentityProvider ) ) ) ?? target.Name;
            return result;
        }
    }
}