using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Reflection.Types;

sealed class IsOf<TIn, T> : ICondition<TIn>
{
	public static IsOf<TIn, T> Default { get; } = new();

	IsOf() {}

	public bool Get(TIn parameter) => parameter is T;
}

sealed class IsOf<T> : Condition<object>
{
	public static IsOf<T> Default { get; } = new();

	IsOf() : base(IsOf<object, T>.Default) {}
}