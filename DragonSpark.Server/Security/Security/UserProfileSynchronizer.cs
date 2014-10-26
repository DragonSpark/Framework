using DragonSpark.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace DragonSpark.Security
{
	public class UserProfileSynchronizer<TUser> : IUserProfileSynchronizer where TUser : UserProfile
	{
		readonly IUserService userService;
		readonly IEnumerable<IClaimsProcessor> processors;

		public UserProfileSynchronizer( IUserService userService, IEnumerable<IClaimsProcessor> processors )
		{
			this.userService = userService;
			this.processors = processors;
		}

		public void Apply( IPrincipal principal )
		{
			principal.As<ClaimsPrincipal>( x =>
			{
				var identity = x.Identity.To<ClaimsIdentity>();

				var user = userService.Ensure<TUser>( principal.Identity.Name );
				Process( user, identity );
			} );
		}

		protected virtual void Process( TUser user, ClaimsIdentity identity )
		{
			user.Claims.Clear();
			user.Claims.AddAll( identity.Claims );

			OnProcessing( user, identity );

			var type = identity.Find( Claims.IdentityProvider );
			type.NotNull( y =>
			{
				var specific = processors.FirstOrDefault( z => z.IdentityProviderName == y );
				var @default = processors.FirstOrDefault( z => string.IsNullOrEmpty( z.IdentityProviderName ) );
				var processor = specific ?? @default;
				processor.NotNull( z => z.Process( identity, user ) );
			} );

			OnProcessed( user, identity );

			userService.Save( user );
		}

		protected virtual void OnProcessing( TUser user, ClaimsIdentity identity )
		{}

		protected virtual void OnProcessed( TUser user, ClaimsIdentity identity )
		{
			user.MembershipNumber = user.MembershipNumber.HasValue ? user.MembershipNumber : userService.GetNextMembershipNumber();

		}
	}
}