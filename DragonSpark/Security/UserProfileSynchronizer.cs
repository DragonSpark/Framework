using DragonSpark.Entity;
using DragonSpark.Extensions;
using DragonSpark.Objects.Synchronization;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Activator = DragonSpark.Runtime.Activator;

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
				user.Claims.Clear();
				user.Claims.AddAll( identity.Claims.Select( y => Activator.Create<IdentityClaim>().SynchronizeFrom( y ) ) );

				var type = identity.Find( Claims.IdentityProvider );
				type.NotNull( y =>
				{
					var specific = processors.FirstOrDefault( z => z.IdentityProviderName == y );
					var @default = processors.FirstOrDefault( z => string.IsNullOrEmpty( z.IdentityProviderName ) );
					var processor = specific ?? @default;
					processor.NotNull( z => z.Process( identity, user ) );
				});

				userService.Save( user );
			} );
		}
	}

	public static class UserServiceExtensions
	{
		public static TUser Ensure<TUser>( this IUserService target, string name, string displayName = null ) where TUser : UserProfile
		{
			var user = target.Get( name ) ?? target.Create( name ).With( x => x.DisplayName = displayName ).With( target.Save );
			var result = user.To<TUser>();
			return result;
		}
	}

	public interface IUserService
	{
		UserProfile Create( string name );

		UserProfile Get( string name );

		void Save( UserProfile user );
	}

	public class UserService<TUser> : IUserService where TUser : UserProfile, new()
	{
		readonly DbContext context;

		public UserService( DbContext context )
		{
			this.context = context;
		}

		public UserProfile Create( string name )
		{
			var result = context.Create<TUser>( x => x.MembershipNumber = DetermineMembershipNumber() );
			result.Name = name;
			return result;
		}

		long DetermineMembershipNumber()
		{
			var result = context.Set<TUser>().LongCount()  + 1;
			return result;
		}

		public UserProfile Get( string name )
		{
			var result = Query().SingleOrDefault( y => y.Name == name );
			return result;
		}

		protected virtual IQueryable<TUser> Query()
		{
			return context.Set<TUser>().Include( y => y.Claims );
		}

		public void Save( UserProfile user )
		{
			user.LastActivity = DateTime.Now;
			context.ApplyChanges( user );
			context.Save();
		}
	}
}