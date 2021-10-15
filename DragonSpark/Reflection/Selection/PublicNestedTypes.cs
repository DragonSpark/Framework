using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using System;

namespace DragonSpark.Reflection.Selection;

public sealed class PublicNestedTypes<T> : ArrayResult<Type>
{
	public static PublicNestedTypes<T> Default { get; } = new PublicNestedTypes<T>();

	PublicNestedTypes() : base(new PublicNestedTypes(A.Type<T>())) {}
}

public sealed class PublicNestedTypes : Instances<Type>
{
	public PublicNestedTypes(Type referenceType) : base(referenceType.GetNestedTypes()) {}
}