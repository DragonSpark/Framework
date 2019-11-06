using System;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Model.Selection.Adapters
{
	public class Condition<T> : Conditions.Condition<T>
	{
		public Condition(Func<T, bool> @delegate) : base(@delegate) {}

		public static implicit operator Condition<T>(Func<T, bool> value) => new Condition<T>(value);
	}

	public class Condition : DelegatedResultCondition
	{
		public Condition(Func<bool> @delegate) : base(@delegate) {}

		public static implicit operator Condition(Func<bool> value) => new Condition(value);
	}
}