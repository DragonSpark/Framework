using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Compose.Model.Validation;

public sealed class OutputConditionSelectionContext<TIn, TOut>
{
	public OutputConditionSelectionContext(ISelect<TIn, TOut> subject, Func<TOut, bool> condition)
		: this(new OtherwiseThrowOutputContext<TIn, TOut>(subject, condition)) {}

	public OutputConditionSelectionContext(OtherwiseThrowOutputContext<TIn, TOut> otherwise)
		=> Otherwise = otherwise;

	public OtherwiseThrowOutputContext<TIn, TOut> Otherwise { get; }
}