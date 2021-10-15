using DragonSpark.Model.Selection;
using System;
using System.Reflection;

namespace DragonSpark.Reflection.Types;

public sealed class TypeSelector : ISelect<TypeInfo, Type>
{
	public static TypeSelector Default { get; } = new TypeSelector();

	TypeSelector() {}

	public Type Get(TypeInfo parameter) => parameter.AsType();
}