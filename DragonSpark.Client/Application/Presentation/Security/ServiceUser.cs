using Microsoft.LightSwitch;
using Microsoft.LightSwitch.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace DragonSpark.Application.Presentation.Security
{
	public sealed class ServiceUser : IUser
	{
		// Fields
		readonly AuthenticationType authenticationType;
		readonly string authenticationTypeString;
		readonly IEnumerable<string> basePermissions;
		readonly IEnumerable<string> roles;

		// Methods
		internal ServiceUser( AuthenticationType authenticationType, bool isAuthenticated, string fullName, IEnumerable<string> roles, IEnumerable<string> permissions )
		{
			Name = string.Empty;
			IsAuthenticated = isAuthenticated;
			authenticationTypeString = Enum.GetName( typeof(AuthenticationType), authenticationType );
			this.authenticationType = authenticationType;
			FullName = fullName;
			this.roles = roles ?? Enumerable.Empty<string>();
			basePermissions = permissions ?? Enumerable.Empty<string>();
		}

		public void DemandPermission( string permissionId )
		{
			if ( !HasPermission( permissionId ) )
			{
				throw new PermissionException( "Resources.PermissionException_UserDoesNotHavePermission" );
			}
		}

		public bool HasPermission( string permissionId )
		{
			return EffectivePermissions.Contains( permissionId );
		}

		public bool IsInRole( string role )
		{
			return roles.Contains( role );
		}

		// Properties
		IEnumerable<string> BasePermissions
		{
			get { return basePermissions; }
		}

		internal IEnumerable<string> EffectivePermissions
		{
			get { return BasePermissions; }
		}

		public string FullName { get; private set; }

		public bool IsAuthenticated { get; private set; }

		AuthenticationType IUser.AuthenticationType
		{
			get { return authenticationType; }
		}

		public string Name { get; private set; }

		string IIdentity.AuthenticationType
		{
			get { return authenticationTypeString; }
		}

		IIdentity IPrincipal.Identity
		{
			get { return this; }
		}
	}
}