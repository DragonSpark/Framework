using System.Data.Entity;
using System.Security.Principal;
using DragonSpark.Entity;
using DragonSpark.Extensions;
using DragonSpark.Objects;
using DragonSpark.Security;

namespace DragonSpark.Server.Client
{
	public class UserProfileFactory<TUserProfile> : Factory<IPrincipal, TUserProfile> where TUserProfile : UserProfile
	{
		readonly DbContext context;

		public UserProfileFactory( DbContext context )
		{
			this.context = context;
		}

		protected override TUserProfile CreateItem( IPrincipal parameter )
		{
			return parameter.Identity.IsAuthenticated ? Load( parameter ) : null;
		}

		TUserProfile Load( IPrincipal parameter )
		{
			var set = context.Set<TUserProfile>();
			var id = parameter.Identity.Name;
			var result = set.Find( id ).To<TUserProfile>();
			context.Include( result, x => x.Claims );
			return result;
		}
	}
}