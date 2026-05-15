using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Compose.Model.Selection;

public sealed class Condition(Func<bool> @delegate) : DelegatedResultCondition(@delegate)
{
	public static implicit operator Condition(Func<bool> value) => new(value);
}