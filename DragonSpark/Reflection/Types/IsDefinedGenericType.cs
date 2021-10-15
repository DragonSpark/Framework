using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Reflection.Types;

sealed class IsDefinedGenericType : AllCondition<Type>
{
	public static IsDefinedGenericType Default { get; } = new IsDefinedGenericType();

	IsDefinedGenericType() : base(IsConstructedGenericType.Default,
	                              IsGenericTypeDefinition.Default.Then().Inverse().Get()) {}
}