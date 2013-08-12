using System;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web.Security;
using DragonSpark.Entity;
using DragonSpark.Extensions;
using DragonSpark.IoC;
using DragonSpark.Security;

namespace DragonSpark.Web.Security
{
    public class SecurityStorageRoleProvider : RoleProvider
	{
		static MethodInfo RetrieveMethod;
		
		string[] DefaultRoles { get; set; }
		Type UserProfileType { get; set; }

		public override string ApplicationName { get; set; }
			 
		public override void Initialize( string name, NameValueCollection config )
		{
			DefaultRoles = config.Get( "defaultRoles" ).Transform( x => x.ToStringArray() ) ?? new string[0];
			ApplicationName = config.Get( "applicationName" ) ?? "/";

			UserProfileType = Type.GetType( config.Get( "userProfileType" ), true );

			RetrieveMethod = typeof(SecurityStorageRoleProvider).GetMethod( "Retrieve", DragonSparkBindingOptions.AllProperties ).MakeGenericMethod( UserProfileType );
		}

		public override string Name
		{
			get { return GetType().Name; }
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Db", Justification = "Following convention from external library." ), Microsoft.Practices.Unity.Dependency]
		public DbContext DbContext { get; set; }

		TUser Retrieve<TUser>( string name ) where TUser : UserProfile
		{
			var result = DbContext.Set<TUser>().SingleOrDefault( y => y.Name == name );
			return result;
		}

		UserProfile RetrieveUser( string name )
		{
			var result = RetrieveMethod.Invoke( this, new object[]{ name } ).To<UserProfile>();
			return result;
		}

		public override bool IsUserInRole( string username, string roleName )
		{
			this.BuildUpOnce();

			var result = RetrieveUser( username ).Transform( x => x.Roles.Contains( roleName ) );
			return result;
		}

		public override string[] GetRolesForUser( string username )
		{
			this.BuildUpOnce();

			var result = RetrieveUser( username ).Transform( x => x.Roles.ToArray() ) ?? DefaultRoles;
			return result;
		}

		public override void CreateRole( string roleName )
		{
			this.BuildUpOnce();

			DbContext.Add( new Role { Name = roleName } );
		}

		public override bool DeleteRole( string roleName, bool throwOnPopulatedRole )
		{
			this.BuildUpOnce();

			var role = RetrieveRole( roleName );
			var result = role != null;
			if ( result )
			{
				DbContext.Remove( role );
			}
			return result;
		}

		Role RetrieveRole( string roleName )
		{
			return DbContext.Set<Role>().SingleOrDefault( x => x.Name == roleName );
		}

		public override bool RoleExists( string roleName )
		{
			this.BuildUpOnce();

			var result = RetrieveRole( roleName ) != null;
			return result;
		}

		public override void AddUsersToRoles( string[] usernames, string[] roleNames )
		{
			this.BuildUpOnce();

			usernames.Apply( x => RetrieveUser( x ).As<UserProfile>( y =>
			{
			    var current = y.Roles ?? Enumerable.Empty<string>();
			    y.ApplyRoles( current.Union( roleNames ) );
			    DbContext.Update( y );
			} ) );
		}

	    public override void RemoveUsersFromRoles( string[] usernames, string[] roleNames )
		{
			this.BuildUpOnce();

			usernames.Apply( x => RetrieveUser( x ).As<UserProfile>( y =>
			{
			    var current = y.Roles ?? Enumerable.Empty<string>();
			    y.ApplyRoles( current.Except( roleNames ) );
			} ) );
		}

		public override string[] GetUsersInRole( string roleName )
		{
			this.BuildUpOnce();

			var result = GetUsersByRole( roleName ).ToArray();
			return result;
		}

		IQueryable<string> GetUsersByRole( string roleName )
		{
			var result = DbContext.Set<UserProfile>().Where( x => ContainsRole( x, roleName ) ).Select( x => x.Name );
			return result;
		}

		static bool ContainsRole( UserProfile userProfile, string roleName )
		{
			var result = userProfile.Roles.Transform( y => y.Contains( roleName ) );
			return result;
		}

		public override string[] GetAllRoles()
		{
			this.BuildUpOnce();

			var result = DbContext.Set<Role>().Select( x => x.Name ).ToArray();
			return result;
		}

		public override string[] FindUsersInRole( string roleName, string usernameToMatch )
		{
			this.BuildUpOnce();

			var result = GetUsersByRole( roleName ).Where( x => x.IndexOf( usernameToMatch, StringComparison.InvariantCultureIgnoreCase ) > 1 ).ToArray();
			return result;
		}
	}
}