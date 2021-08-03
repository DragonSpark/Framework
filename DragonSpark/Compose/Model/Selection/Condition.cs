using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Compose.Model.Selection
{
	public class Condition<T> : DragonSpark.Model.Selection.Conditions.Condition<T>
	{
		public static implicit operator Condition<T>(Func<T, bool> value) => new Condition<T>(value);

		public Condition(Func<T, bool> @delegate) : base(@delegate) {}
	}

	public class Condition : DelegatedResultCondition
	{
		public static implicit operator Condition(Func<bool> value) => new Condition(value);

		public Condition(Func<bool> @delegate) : base(@delegate) {}
	}
}