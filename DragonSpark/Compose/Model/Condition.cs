using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Compose.Model
{
	public class Condition<T> : DragonSpark.Model.Selection.Conditions.Condition<T>
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