using System.Security.Claims;
using DragonSpark.Application.Communication.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using DragonSpark.Extensions;
using DragonSpark.Objects.Synchronization;
using Microsoft.Practices.Prism;
using Activator = DragonSpark.Runtime.Activator;

namespace DragonSpark.Application.Communication.Security
{
	/*[Singleton( typeof(IClaimProcessorRegistry), Priority = Priority.Lowest )]
	public class ClaimProcessorRegistry : IClaimProcessorRegistry
	{
		readonly IDictionary<string, IClaimsIdentityProcessor> binders;

		public ClaimProcessorRegistry( IDictionary<string, IClaimsIdentityProcessor> binders  )
		{
			this.binders = binders;
		}

		public IClaimsIdentityProcessor Retrieve( string providerName )
		{
			var result = binders.ContainsKey( providerName ) ? binders[ providerName ] : null;
			return result;
		}
	}*/

    /*public class DefaultClaimsProcessorFactory : FactoryBase
	{
		protected override object Create( IUnityContainer container, Type type, string buildName )
		{
			var result = new ClaimsProcessor( null, new[]{  } )
		}
	}*/

    public class ClaimsIdentitySynchronizer<TUser> : IIdentitySynchronizer where TUser : ApplicationUser
	{
		readonly DbContext context;
		readonly IEnumerable<IClaimsProcessor> processors;

		public ClaimsIdentitySynchronizer( DbContext context, IEnumerable<IClaimsProcessor> processors )
		{
			this.context = context;
			this.processors = processors;
		}

		public void Apply( IPrincipal principal )
		{
			principal.As<ClaimsPrincipal>( x =>
			{
				var identity = x.Identity.To<ClaimsIdentity>();

				var name = principal.Identity.DetermineUniqueName();
				var user = context.Set<TUser>().SingleOrDefault( y => y.Name == name ) ?? CreateUser( principal );
				user.Claims.Clear();
				user.Claims.AddRange( identity.Claims.Select( y => Activator.Create<IdentityClaim>().SynchronizeFrom( y ) ) );
				
				var specific = identity.Claims.FirstOrDefault( y => y.Type == Claims.IdentityProvider ).Transform( y => processors.FirstOrDefault( z => z.IdentityProviderName == y.Value ) );
				var processor = specific ?? processors.FirstOrDefault( y => string.IsNullOrEmpty( y.IdentityProviderName ) );
				processor.NotNull( y => y.Process( identity, user ) );

				user.LastActivity = DateTime.Now;
				context.ApplyChanges( user );
				context.SaveChanges();
			} );
		}

		TUser CreateUser( IPrincipal principal )
		{
			var result = UserFactory<TUser>.Instance.Create( principal );
			result.Name = principal.Identity.DetermineUniqueName();
			result.MembershipNumber = ResolveMembershipNumber();
			result.JoinedDate = DateTime.Now;
			return result;
		}

		long ResolveMembershipNumber()
		{
			var result = context.Set<TUser>().LongCount()  + 1;
			return result;
		}
	}
}