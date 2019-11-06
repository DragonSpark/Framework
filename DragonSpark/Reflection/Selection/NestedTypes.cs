using System;
using System.Reflection;
using DragonSpark.Model.Sequences;
using DragonSpark.Reflection.Types;

namespace DragonSpark.Reflection.Selection
{
	public sealed class NestedTypes<T> : ArrayResult<Type>
	{
		public static NestedTypes<T> Default { get; } = new NestedTypes<T>();

		NestedTypes() : base(new NestedTypes(Type<T>.Metadata)) {}
	}

	public sealed class NestedTypes : ArrayInstance<Type>
	{
		public NestedTypes(TypeInfo referenceType) : base(referenceType.DeclaredNestedTypes) {}
	}
}