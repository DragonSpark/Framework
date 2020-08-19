using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Security.Identity
{
	public sealed class UserEqualityComparer : IEqualityComparer<IdentityUser>
	{
		public static UserEqualityComparer Default { get; } = new UserEqualityComparer();

		UserEqualityComparer() {}

		public bool Equals(IdentityUser? x, IdentityUser? y)
			=> ReferenceEquals(x, y)
			   || !ReferenceEquals(x, null)
			   && !ReferenceEquals(y, null)
			   && string.Equals(x.UserName, y.UserName, StringComparison.OrdinalIgnoreCase);

		public int GetHashCode(IdentityUser obj) => StringComparer.OrdinalIgnoreCase.GetHashCode(obj.UserName);
	}
}