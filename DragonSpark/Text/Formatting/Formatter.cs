using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Reflection.Types;
using System;

namespace DragonSpark.Text.Formatting;

public class Formatter : Conditional<object, IFormattable>, IFormatter
{
	public Formatter(IConditional<object, IFormattable> condition)
		: this(condition.Condition, condition) {}

	public Formatter(ICondition<object> condition, ISelect<object, IFormattable> source)
		: base(condition, source) {}
}

public sealed class Formatter<T> : Formatter
{
	public Formatter(ISelect<object, IFormattable> condition) : base(IsOf<T>.Default, condition) {}
}