using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Reflection.Types
{
	sealed class IsGenericTypeDefinition : Condition<Type>
	{
		public static IsGenericTypeDefinition Default { get; } = new IsGenericTypeDefinition();

		IsGenericTypeDefinition() : base(x => x.IsGenericTypeDefinition) {}
	}
}