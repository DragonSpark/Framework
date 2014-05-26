using DragonSpark.Objects;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Claim = System.Security.Claims.Claim;

namespace DragonSpark.Security
{
	public class UserProfile
	{
		[Key]
		public string Name { get; set; }

		public string DisplayName { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public DateTimeOffset LastActivity { get; set; }

		[CurrentTimeOffsetDefault]
		public DateTimeOffset JoinedDate { get; set; }

		public long? MembershipNumber { get; set; }

		[NotMapped]
		public Collection<Claim> Claims
		{
			get { return claims; }
		}	readonly Collection<Claim> claims = new Collection<Claim>();
	}
}