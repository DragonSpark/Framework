using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Security.Identity.Profile;

public sealed class UserEqualityComparer : IEqualityComparer<IdentityUser>
{
	public static UserEqualityComparer Default { get; } = new();

	UserEqualityComparer() {}

	public bool Equals(IdentityUser? x, IdentityUser? y)
		=> ReferenceEquals(x, y) || x != null && y != null && x?.Id == y?.Id;

	public int GetHashCode(IdentityUser obj) => StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Id.ToString());
}