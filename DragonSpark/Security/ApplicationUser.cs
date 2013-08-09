using DragonSpark.Extensions;
using DragonSpark.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.ServiceModel.DomainServices.Server.ApplicationServices;

namespace DragonSpark.Application.Communication.Security
{
	public partial class ApplicationUser : UserBase
	{
		public string DisplayName { get; set; }

		public string FirstName { get; set; }
		
		public string LastName { get; set; }

		public DateTime? LastActivity { get; set; }

		public long? MembershipNumber { get; set; }

		public DateTime? JoinedDate { get; set; }

		[Editable(false), ProfileUsage(IsExcluded=true)]
		public IEnumerable<string> Permissions { get; set; }

		[Display( Name = "Assigned Permissions", Description = "Permissions that this user has." )]
		public string PermissionsSource
		{
			get { return string.Join( ";", Permissions ?? Enumerable.Empty<string>() ); }
			set { Permissions = value.ToStringArray(); }
		}

		[DefaultPropertyValue( true )]
		public bool IsEnabled { get; set; }

		[Display( Name = "Assigned Roles", Description = "Roles that this user is assigned to." )]
		public string RolesSource
		{
			get { return string.Join( ";", Roles ?? Enumerable.Empty<string>() ); }
			set { Roles = value.ToStringArray(); }
		}

		[Include, Association( "ClaimsUser", "Name", "UserName" )]
		public Collection<IdentityClaim> Claims
		{
			get { return claims; }
		}	readonly Collection<IdentityClaim> claims = new Collection<IdentityClaim>();
	}
}