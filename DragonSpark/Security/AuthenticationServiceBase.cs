using DragonSpark.Application.Communication.Entity;
using DragonSpark.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IdentityModel.Services;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.ServiceModel.DomainServices.Server.ApplicationServices;
using System.Web;

namespace DragonSpark.Application.Communication.Security
{
    public abstract class AuthenticationServiceBase<TStorage, TUser> : DbDomainService<TStorage>, IAuthentication<TUser> where TUser : ApplicationUser, new() where TStorage : DbContext, new()
	{
		static readonly IEnumerable<string> DefaultRoles = new string[0];
		static readonly IPrincipal DefaultPrincipal = new GenericPrincipal( new GenericIdentity(string.Empty), DefaultRoles.ToArray() );

        public TUser Login( string userName, string password, bool isPersistent, string customData )
		{
			throw new NotSupportedException( "Authentication is performed through an external identity provider." );
		}

		public TUser Logout()
		{
			FederatedAuthentication.WSFederationAuthenticationModule.SignOut( false );
			var result = GetAnonymousUser();
			return result;
		}

		public TUser GetUser()
		{
			var result = HttpContext.Current.User.Identity.AsTo<ClaimsIdentity,TUser>( x => x.IsAuthenticated ? GetAuthenticatedUser( x ) : GetAnonymousUser() );
			return result;
		}

		protected virtual TUser GetAuthenticatedUser( ClaimsIdentity identity )
		{
			var name = identity.DetermineUniqueName();
			var result = DbContext.Set<TUser>().Single( x => x.Name == name );
			Process( result );
			return result;
		}

		protected virtual void Process( TUser result )
		{
			result.ApplyRoles();
		}

		public void UpdateUser( TUser user )
		{
			DbContext.ApplyChanges( user ).LastActivity = DateTime.Now;
		}

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Following pattern of RIA Services." )]
        protected virtual TUser GetAnonymousUser()
		{
			var result = UserFactory<TUser>.Instance.Create( DefaultPrincipal );
			return result;
		}
	}
}
