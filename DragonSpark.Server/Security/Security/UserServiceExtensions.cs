using System.Security.Principal;
using DragonSpark.Extensions;

namespace DragonSpark.Server.Legacy.Security.Security
{
	public static class UserServiceExtensions
	{
		public static TUser Get<TUser>( this IUserService target, IPrincipal principal ) where TUser : UserProfile
		{
			var user = principal.Identity.IsAuthenticated ? target.Get( principal.Identity.Name ) : target.GetAnonymous();
			var result = user.To<TUser>();
			return result;
		}

		public static TUser Ensure<TUser>( this IUserService target, string name, string displayName = null ) where TUser : UserProfile
		{
			var user = target.Get( name ) ?? target.Create( name ).With( x => x.DisplayName = displayName ).With( target.Save );
			var result = user.To<TUser>();
			return result;
		}
	}
}