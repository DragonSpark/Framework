using System;
using System.Reflection;
using DragonSpark.Model.Selection;

namespace DragonSpark.Reflection.Types
{
	public sealed class TypeSelector : ISelect<TypeInfo, Type>
	{
		public static TypeSelector Default { get; } = new TypeSelector();

		TypeSelector() {}

		public Type Get(TypeInfo parameter) => parameter.AsType();
	}
}