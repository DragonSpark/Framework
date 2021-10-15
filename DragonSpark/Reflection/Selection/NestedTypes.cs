using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using System;
using System.Reflection;

namespace DragonSpark.Reflection.Selection;

public sealed class NestedTypes<T> : ArrayResult<Type>
{
	public static NestedTypes<T> Default { get; } = new NestedTypes<T>();

	NestedTypes() : base(new NestedTypes(A.Metadata<T>())) {}
}

public sealed class NestedTypes : Instances<Type>
{
	public NestedTypes(TypeInfo referenceType) : base(referenceType.DeclaredNestedTypes) {}
}