using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using JetBrains.Annotations;
using System;

namespace DragonSpark.Reflection.Selection;

[UsedImplicitly]
public sealed class PublicNestedTypes<T> : ArrayResult<Type>
{
	public static PublicNestedTypes<T> Default { get; } = new();

	PublicNestedTypes() : base(new PublicNestedTypes(A.Type<T>())) {}
}

public sealed class PublicNestedTypes : Instances<Type>
{
	public PublicNestedTypes(Type referenceType) : base(referenceType.GetNestedTypes()) {}
}