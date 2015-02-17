using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using DragonSpark.Extensions;

namespace DragonSpark.Server.Legacy.Security.Security
{
	public class UserService<TUser> : IUserService where TUser : UserProfile, new()
	{
		readonly DbContext context;

		public UserService( DbContext context )
		{
			this.context = context;
		}

		public long GetNextMembershipNumber()
		{
			var result = context.Set<TUser>().Where( x => x.MembershipNumber.HasValue ).LongCount() + 1;
			return result;
		}

		public virtual UserProfile Create( string name )
		{
			var result = context.Create<TUser>();
			result.Name = name;
			return result;
		}

		public virtual UserProfile Get( string name )
		{
			var result = Query().SingleOrDefault( y => y.Name == name );

			if ( result != null && name == Thread.CurrentPrincipal.Identity.Name )
			{
				var items = Thread.CurrentPrincipal.Identity.AsTo<ClaimsIdentity, IEnumerable<Claim>>( x => x.Claims );
				result.Claims.AddAll( items );
			}

			return result;
		}

		public virtual UserProfile GetAnonymous()
		{
			return null;
		}

		protected virtual IQueryable<TUser> Query()
		{
			var result = context.Set<TUser>();
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