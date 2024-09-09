using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Types;
using System;

namespace DragonSpark.Compose.Model.Validation;

public sealed class ConditionalOutputSelectionContext<TIn, TOut>
{
	readonly ISelect<TIn, TOut> _subject;

	public ConditionalOutputSelectionContext(ISelect<TIn, TOut> subject) => _subject = subject;

	public AssignedOutputConditionSelectionComposer<TIn, TOut> IsAssigned => new(_subject);

	public OutputConditionSelectionComposer<TIn, TOut> IsOf<T>() => Is(IsOf<TOut, T>.Default);

	public OutputConditionSelectionComposer<TIn, TOut> Is(ISelect<TOut, bool> condition) => Is(condition.Get);

	public OutputConditionSelectionComposer<TIn, TOut> Is(Func<TOut, bool> condition) => new(_subject, condition);
}