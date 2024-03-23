using DragonSpark.Model.Sequences;
using JetBrains.Annotations;
using System;

namespace DragonSpark.Reflection.Selection;

[UsedImplicitly]
public sealed class AllTypesInSameNamespace<T> : ArrayResult<Type>
{
	public static AllTypesInSameNamespace<T> Default { get; } = new();

	AllTypesInSameNamespace() : base(new AllTypesInSameNamespace(typeof(T))) {}
}

public sealed class AllTypesInSameNamespace : ArrayResult<Type>
{
	public AllTypesInSameNamespace(Type referenceType)
		: base(new TypesInSameNamespace(referenceType, new AllAssemblyTypes(referenceType).Get().Open())) {}
}