using System;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Reflection.Types
{
	sealed class IsClass : Condition<Type>
	{
		public static IsClass Default { get; } = new IsClass();

		IsClass() : base(x => x.IsClass) {}
	}
}