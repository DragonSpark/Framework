using DragonSpark.Model.Results;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Model;

internal class Class1 {}

public interface IIdentityAware : IResult<Guid> {}

public sealed class IdentityAwareEqualityComparer : IEqualityComparer<IIdentityAware>
{
	public static IdentityAwareEqualityComparer Default { get; } = new();

	IdentityAwareEqualityComparer() {}

	public bool Equals(IIdentityAware? x, IIdentityAware? y)
		=> ReferenceEquals(x, y) || !ReferenceEquals(x, null) && !ReferenceEquals(y, null) &&
		   x.GetType() == y.GetType() && x.Get().Equals(y.Get());

	public int GetHashCode(IIdentityAware obj) => obj.Get().GetHashCode();
}