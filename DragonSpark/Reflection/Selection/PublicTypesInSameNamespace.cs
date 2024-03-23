using DragonSpark.Model.Sequences;
using JetBrains.Annotations;
using System;

namespace DragonSpark.Reflection.Selection;

[UsedImplicitly]
public sealed class PublicTypesInSameNamespace<T> : ArrayResult<Type>
{
	public static PublicTypesInSameNamespace<T> Default { get; } = new();

	PublicTypesInSameNamespace() : base(new PublicTypesInSameNamespace(typeof(T))) {}
}

public sealed class PublicTypesInSameNamespace : ArrayResult<Type>
{
	public PublicTypesInSameNamespace(Type referenceType)
		: base(new TypesInSameNamespace(referenceType, new PublicAssemblyTypes(referenceType).Get().Open())) {}
}