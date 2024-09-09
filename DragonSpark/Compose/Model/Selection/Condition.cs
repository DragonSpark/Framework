using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Compose.Model.Selection;

/*
public class Condition<T> : DragonSpark.Model.Selection.Conditions.Condition<T>
{
	public static implicit operator Condition<T>(Func<T, bool> value) => new(value);

	public Condition(Func<T, bool> @delegate) : base(@delegate) {}
}
*/

public sealed class Condition(Func<bool> @delegate) : DelegatedResultCondition(@delegate)
{
	public static implicit operator Condition(Func<bool> value) => new(value);
}