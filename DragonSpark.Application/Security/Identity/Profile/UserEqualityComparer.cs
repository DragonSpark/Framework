using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Security.Identity.Profile
{
	public sealed class UserEqualityComparer : IEqualityComparer<IdentityUser>
	{
		public static UserEqualityComparer Default { get; } = new UserEqualityComparer();

		UserEqualityComparer() {}

		public bool Equals(IdentityUser? x, IdentityUser? y)
			=> ReferenceEquals(x, y)
			   ||
			   x != null
			   && y != null
			   && string.Equals(x.UserName, y.UserName, StringComparison.OrdinalIgnoreCase);

		public int GetHashCode(IdentityUser obj) => StringComparer.OrdinalIgnoreCase.GetHashCode(obj.UserName);
	}
}