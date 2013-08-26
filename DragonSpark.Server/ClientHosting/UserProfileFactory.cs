using DragonSpark.Extensions;
using DragonSpark.Objects;
using DragonSpark.Security;
using System.Security.Principal;

namespace DragonSpark.Server.ClientHosting
{
	public class UserProfileFactory<TUserProfile> : Factory<IPrincipal, TUserProfile> where TUserProfile : UserProfile
	{
		readonly IUserService userService;

		public UserProfileFactory( IUserService userService )
		{
			this.userService = userService;
		}

		protected override TUserProfile CreateItem( IPrincipal parameter )
		{
			var result = parameter.Identity.IsAuthenticated ? userService.Get( parameter.Identity.Name ).To<TUserProfile>() : null;
			return result;
		}
	}
}