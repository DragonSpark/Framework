using System.Collections.Generic;

namespace DragonSpark.Reflection.Types;

public sealed class NeverEqualityComparer<T> : IEqualityComparer<T>
{
	public static NeverEqualityComparer<T> Default { get; } = new();

	NeverEqualityComparer() {}

	public bool Equals(T? x, T? y) => false;

	public int GetHashCode(T obj) => obj!.GetHashCode();
}