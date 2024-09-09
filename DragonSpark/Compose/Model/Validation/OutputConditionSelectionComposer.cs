using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Compose.Model.Validation;

public sealed class OutputConditionSelectionComposer<TIn, TOut>
{
	public OutputConditionSelectionComposer(ISelect<TIn, TOut> subject, Func<TOut, bool> condition)
		: this(new OtherwiseThrowOutputComposer<TIn, TOut>(subject, condition)) {}

	public OutputConditionSelectionComposer(OtherwiseThrowOutputComposer<TIn, TOut> otherwise)
		=> Otherwise = otherwise;

	public OtherwiseThrowOutputComposer<TIn, TOut> Otherwise { get; }
}