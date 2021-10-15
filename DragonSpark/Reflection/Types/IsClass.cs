using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Reflection.Types;

sealed class IsClass : Condition<Type>
{
	public static IsClass Default { get; } = new IsClass();

	IsClass() : base(x => x.IsClass) {}
}