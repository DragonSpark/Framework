using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Reflection.Types;

sealed class IsConstructedGenericType : Condition<Type>
{
	public static IsConstructedGenericType Default { get; } = new IsConstructedGenericType();

	IsConstructedGenericType() : base(x => x.IsConstructedGenericType) {}
}