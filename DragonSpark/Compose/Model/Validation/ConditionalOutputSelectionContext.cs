using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Types;
using System;

namespace DragonSpark.Compose.Model.Validation
{
	public sealed class ConditionalOutputSelectionContext<TIn, TOut>
	{
		readonly ISelect<TIn, TOut> _subject;

		public ConditionalOutputSelectionContext(ISelect<TIn, TOut> subject) => _subject = subject;

		public AssignedOutputConditionSelectionContext<TIn, TOut> IsAssigned
			=> new AssignedOutputConditionSelectionContext<TIn, TOut>(_subject);

		public OutputConditionSelectionContext<TIn, TOut> IsOf<T>() => Is(IsOf<TOut, T>.Default);

		public OutputConditionSelectionContext<TIn, TOut> Is(ISelect<TOut, bool> condition) => Is(condition.Get);

		public OutputConditionSelectionContext<TIn, TOut> Is(Func<TOut, bool> condition)
			=> new OutputConditionSelectionContext<TIn, TOut>(_subject, condition);
	}
}