using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DragonSpark.Reflection.Types;

public class TypeComparer<T> : IEqualityComparer<T>
{
	public static TypeComparer<T> Default { get; } = new TypeComparer<T>();

	TypeComparer() {}

	public bool Equals([AllowNull]T x, [AllowNull]T y) => x?.GetType() == y?.GetType();

	public int GetHashCode(T obj) => 0;
}

// TODO

public sealed class NeverEqualityComparer<T> : IEqualityComparer<T>
{
	public static NeverEqualityComparer<T> Default { get; } = new();

	NeverEqualityComparer() {}

	public bool Equals(T? x, T? y) => false;

	public int GetHashCode(T obj) => obj!.GetHashCode();
}