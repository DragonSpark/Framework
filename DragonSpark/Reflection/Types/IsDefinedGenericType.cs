using System;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Reflection.Types
{
	sealed class IsDefinedGenericType : AllCondition<Type>
	{
		public static IsDefinedGenericType Default { get; } = new IsDefinedGenericType();

		IsDefinedGenericType() : base(IsConstructedGenericType.Default,
		                              IsGenericTypeDefinition.Default.Then().Inverse().Get()) {}
	}
}