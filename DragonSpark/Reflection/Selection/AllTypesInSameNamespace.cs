using DragonSpark.Model.Sequences;
using System;

namespace DragonSpark.Reflection.Selection
{
	public sealed class AllTypesInSameNamespace<T> : ArrayResult<Type>
	{
		public static AllTypesInSameNamespace<T> Default { get; } = new AllTypesInSameNamespace<T>();

		AllTypesInSameNamespace() : base(new AllTypesInSameNamespace(typeof(T))) {}
	}

	public sealed class AllTypesInSameNamespace : ArrayResult<Type>
	{
		public AllTypesInSameNamespace(Type referenceType)
			: base(new TypesInSameNamespace(referenceType, new AllAssemblyTypes(referenceType).Get().Open())) {}
	}
}