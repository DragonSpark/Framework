using DragonSpark.Objects;
using DragonSpark.Security;
using System.Security.Principal;

namespace DragonSpark.Server.ClientHosting
{
	public class UserProfileFactory : Factory<IPrincipal, UserProfile>
	{
		readonly IUserService userService;

		public UserProfileFactory( IUserService userService )
		{
			this.userService = userService;
		}

		protected override UserProfile CreateItem( IPrincipal parameter )
		{
			var result = userService.Get<UserProfile>( parameter );
			return result;
		}
	}
}