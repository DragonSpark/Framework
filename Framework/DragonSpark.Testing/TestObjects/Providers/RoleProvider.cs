using System;
using System.Collections.Generic;
using System.Linq;
using DragonSpark.Extensions;

namespace DragonSpark.Testing.TestObjects.Providers
{
	class RoleProvider : System.Web.Security.RoleProvider
	{
		readonly List<string> roles = new List<string>();
		readonly Dictionary<string,List<string>> cache = new Dictionary<string, List<string>>();

		public override bool IsUserInRole( string username, string roleName )
		{
			var result = ResolveRoles( username ).Contains( roleName );
			return result;
		}

		public override string[] GetRolesForUser( string username )
		{
			var result = ResolveRoles( username ).ToArray();
			return result;
		}

		List<string> ResolveRoles( string username )
		{
			return cache.Ensure( username, item => new List<string>() );
		}

		public override void CreateRole( string roleName )
		{
			roles.Add( roleName );
		}

		public override bool DeleteRole( string roleName, bool throwOnPopulatedRole )
		{
			var result = roles.Remove( roleName );
			return result;
		}

		public override bool RoleExists( string roleName )
		{
			var result = roles.Contains( roleName );
			return result;
		}

		public override void AddUsersToRoles( string[] usernames, string[] roleNames )
		{
			usernames.Apply( item => ResolveRoles( item ).AddRange( roleNames ) );
		}

		public override void RemoveUsersFromRoles( string[] usernames, string[] roleNames )
		{
			usernames.Apply( item => ResolveRoles( item ).RemoveAll( roleNames.Contains ) );
		}

		public override string[] GetUsersInRole( string roleName )
		{
			throw new NotImplementedException();
		}

		public override string[] GetAllRoles()
		{
			var result = roles.ToArray();
			return result;
		}

		public override string[] FindUsersInRole( string roleName, string usernameToMatch )
		{
			var query = from key in cache.Keys
			            where cache[ key ].Contains( roleName )
			            select key;
			var result = query.ToArray();
			return result;
		}

		public override string ApplicationName
		{
			get { return "DragonSpark Framework"; }
			set { throw new NotImplementedException(); }
		}
	}
}