using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Reflection.Types
{
	sealed class IsOf<TIn, T> : ICondition<TIn>
	{
		public static IsOf<TIn, T> Default { get; } = new IsOf<TIn, T>();

		IsOf() {}

		public bool Get(TIn parameter) => parameter is T;
	}

	sealed class IsOf<T> : Condition<object>
	{
		public static IsOf<T> Default { get; } = new IsOf<T>();

		IsOf() : base(IsOf<object, T>.Default) {}
	}
}