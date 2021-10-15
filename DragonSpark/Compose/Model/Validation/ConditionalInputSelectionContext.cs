using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Types;
using System;

namespace DragonSpark.Compose.Model.Validation;

public sealed class ConditionalInputSelectionContext<TIn, TOut>
{
	readonly ISelect<TIn, TOut> _subject;

	public ConditionalInputSelectionContext(ISelect<TIn, TOut> subject) => _subject = subject;

	public AssignedInputConditionSelectionContext<TIn, TOut> IsAssigned
		=> new AssignedInputConditionSelectionContext<TIn, TOut>(_subject);

	public InputConditionSelectionContext<TIn, TOut> IsOf<T>() => Is(IsOf<TIn, T>.Default);

	public InputConditionSelectionContext<TIn, TOut> Is(ISelect<TIn, bool> condition) => Is(condition.Get);

	public InputConditionSelectionContext<TIn, TOut> Is(Func<TIn, bool> condition)
		=> new InputConditionSelectionContext<TIn, TOut>(_subject, condition);
}