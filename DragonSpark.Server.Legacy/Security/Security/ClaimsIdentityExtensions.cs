using System.Security.Claims;
using DragonSpark.Extensions;

namespace DragonSpark.Server.Legacy.Security.Security
{
    public static class ClaimsIdentityExtensions
    {
		public static string Find( this ClaimsIdentity target, string type )
		{
			var result = target.FindFirst( y => y.Type == type ).Transform( z => z.Value );
			return result;
		}

        /*public static string DetermineUniqueName( this IIdentity target )
        {
            var result = target.AsTo<ClaimsIdentity,string>( x => string.Concat( x.Find( Claims.IdentityProvider ), "-", new[] {ClaimTypes.NameIdentifier, ClaimTypes.Upn, ClaimTypes.Name, ClaimTypes.Email }.Select( x.Find ).NotNull().FirstOrDefault() ) ) ?? target.Name;
            return result;
        }*/
    }
}