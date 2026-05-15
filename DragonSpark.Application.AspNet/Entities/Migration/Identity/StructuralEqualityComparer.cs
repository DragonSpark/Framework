using System.Collections;
using System.Collections.Generic;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

sealed class StructuralEqualityComparer<TFrom, TTo> : IEqualityComparer<object>
{
	public static StructuralEqualityComparer<TFrom, TTo> Default { get; } = new();

	StructuralEqualityComparer() : this(StructuralComparisons.StructuralEqualityComparer) {}

	readonly IEqualityComparer _previous;

	public StructuralEqualityComparer(IEqualityComparer previous) => _previous = previous;

	bool IEqualityComparer<object>.Equals(object? x, object? y) => _previous.Equals(x, y);

	public int GetHashCode(object obj) => _previous.GetHashCode(obj);
}