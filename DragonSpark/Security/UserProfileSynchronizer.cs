using DragonSpark.Entity;
using DragonSpark.Extensions;
using DragonSpark.Objects.Synchronization;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
				Process( user, identity );
			} );
		}

		protected virtual void Process( TUser user, ClaimsIdentity identity )
		{
			user.Claims.Clear();
			user.Claims.AddAll( identity.Claims.Select( y => Activator.Create<IdentityClaim>().SynchronizeFrom( y ) ) );

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
			switch ( user.MembershipNumber )
			{
				case 0:
					user.MembershipNumber = userService.GetNextMembershipNumber();
					break;
			}
		}
	}

	public static class UserServiceExtensions
	{
		public static TUser Get<TUser>( this IUserService target, IPrincipal principal ) where TUser : UserProfile
		{
			var result = principal.Identity.IsAuthenticated ? target.Get( principal.Identity.Name ).To<TUser>() : null;
			return result;
		}

		public static TUser Ensure<TUser>( this IUserService target, string name, string displayName = null ) where TUser : UserProfile
		{
			var user = target.Get( name ) ?? target.Create( name ).With( x => x.DisplayName = displayName ).With( target.Save );
			var result = user.To<TUser>();
			return result;
		}
	}

	public interface IUserService
	{
		long GetNextMembershipNumber();

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

		public long GetNextMembershipNumber()
		{
			var result = context.Set<TUser>().Where( x => x.Claims.Any() ).LongCount() + 1;
			return result;
		}

		public UserProfile Create( string name )
		{
			var result = context.Create<TUser>();
			result.Name = name;
			return result;
		}

		public UserProfile Get( string name )
		{
			var result = Query().SingleOrDefault( y => y.Name == name );
			return result;
		}

		protected virtual IQueryable<TUser> Query()
		{
			var result = context.Set<TUser>().Include( y => y.Claims );
			return result;
		}

		public void Save( UserProfile user )
		{
			user.LastActivity = DateTime.Now;

			context.ApplyChanges( user );

			context.Save();
		}
	}
}