using DragonSpark.Objects;
using System;
using System.Globalization;
using System.Security.Principal;
using System.Web.Security;
using Activator = DragonSpark.Runtime.Activator;

namespace DragonSpark.Application.Communication.Security
{
    public class UserFactory<TUser> : Factory<IPrincipal,TUser> where TUser : ApplicationUser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Used for convenience." )]
        public static UserFactory<TUser> Instance
        {
            get { return InstanceField; }
        }	static readonly UserFactory<TUser> InstanceField = new UserFactory<TUser>();

        protected virtual TUser CreateUser()
        {
            var result = Activator.Create<TUser>();
            return result;
        }

        protected override TUser CreateItem( IPrincipal source )
        {
            var user = CreateUser();
            if ( user == null )
            {
                throw new InvalidOperationException( string.Format( CultureInfo.InvariantCulture, "Resources Application Services Create User Cannot Be Null: {0}.", new[] { (object)GetType() } ) );
            }
            user.Name = source.Identity.DetermineUniqueName() ?? string.Empty;
            user.ApplyRoles( Roles.GetRolesForUser( user.Name ) );
            return user;
        }
    }
}