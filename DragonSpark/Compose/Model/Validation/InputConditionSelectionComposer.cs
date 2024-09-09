using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Compose.Model.Validation;

public sealed class InputConditionSelectionComposer<TIn, TOut>
{
	public InputConditionSelectionComposer(ISelect<TIn, TOut> subject, Func<TIn, bool> condition)
		: this(new OtherwiseThrowInputComposer<TIn, TOut>(subject, condition)) {}

	public InputConditionSelectionComposer(OtherwiseThrowInputComposer<TIn, TOut> otherwise)
		=> Otherwise = otherwise;

	public OtherwiseThrowInputComposer<TIn, TOut> Otherwise { get; }
}