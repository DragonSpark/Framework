using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Types;
using System;

namespace DragonSpark.Compose.Model.Validation;

public sealed class ConditionalInputSelectionContext<TIn, TOut>
{
	readonly ISelect<TIn, TOut> _subject;

	public ConditionalInputSelectionContext(ISelect<TIn, TOut> subject) => _subject = subject;

	public AssignedInputConditionSelectionComposer<TIn, TOut> IsAssigned => new(_subject);

	public InputConditionSelectionComposer<TIn, TOut> IsOf<T>() => Is(IsOf<TIn, T>.Default);

	public InputConditionSelectionComposer<TIn, TOut> Is(ISelect<TIn, bool> condition) => Is(condition.Get);

	public InputConditionSelectionComposer<TIn, TOut> Is(Func<TIn, bool> condition) => new(_subject, condition);
}