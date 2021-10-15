using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Types;
using System;

namespace DragonSpark.Compose.Model.Validation;

public sealed class UnlessUsingContext<TIn, TOut>
{
	readonly ISelect<TIn, TOut> _other;
	readonly ISelect<TIn, TOut> _subject;

	public UnlessUsingContext(ISelect<TIn, TOut> subject, ISelect<TIn, TOut> other)
	{
		_subject = subject;
		_other   = other;
	}

	public Selector<TIn, TOut> IsOf<T>() => Results(IsOf<TOut, T>.Default.Get);

	public Selector<TIn, TOut> ResultsInAssigned() => Results(Is.Assigned<TOut>());

	public Selector<TIn, TOut> Results(Func<TOut, bool> @in)
		=> new ValidatedResult<TIn, TOut>(@in, _other.Get, _subject.Get).Then();
}