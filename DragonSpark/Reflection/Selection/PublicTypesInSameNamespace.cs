using DragonSpark.Model.Sequences;
using System;

namespace DragonSpark.Reflection.Selection
{
	public sealed class PublicTypesInSameNamespace<T> : ArrayResult<Type>
	{
		public static PublicTypesInSameNamespace<T> Default { get; } = new PublicTypesInSameNamespace<T>();

		PublicTypesInSameNamespace() : base(new PublicTypesInSameNamespace(typeof(T))) {}
	}

	public sealed class PublicTypesInSameNamespace : ArrayResult<Type>
	{
		public PublicTypesInSameNamespace(Type referenceType)
			: base(new TypesInSameNamespace(referenceType, new PublicAssemblyTypes(referenceType).Get().Open())) {}
	}
}