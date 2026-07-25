using System.Collections;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

sealed class StructuralEqualityComparer : IEqualityComparer<object?>
{
	public readonly static StructuralEqualityComparer Default = new();

	StructuralEqualityComparer() : this(StructuralComparisons.StructuralEqualityComparer) {}

	readonly IEqualityComparer _previous;

	public StructuralEqualityComparer(IEqualityComparer previous) => _previous = previous;

	bool IEqualityComparer<object?>.Equals(object? x, object? y)
		=> ReferenceEquals(x, y) || x?.GetHashCode() == y?.GetHashCode() || _previous.Equals(x, y);
	public int GetHashCode(object obj) => _previous.GetHashCode(obj);
}