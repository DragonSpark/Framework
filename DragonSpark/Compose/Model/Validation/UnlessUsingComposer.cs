using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Types;
using System;

namespace DragonSpark.Compose.Model.Validation;

public sealed class UnlessUsingComposer<TIn, TOut>
{
	readonly ISelect<TIn, TOut> _other;
	readonly ISelect<TIn, TOut> _subject;

	public UnlessUsingComposer(ISelect<TIn, TOut> subject, ISelect<TIn, TOut> other)
	{
		_subject = subject;
		_other   = other;
	}

	public Composer<TIn, TOut> IsOf<T>() => Results(IsOf<TOut, T>.Default.Get);

	public Composer<TIn, TOut> ResultsInAssigned() => Results(Is.Assigned<TOut>());

	public Composer<TIn, TOut> Results(Func<TOut, bool> @in)
		=> new ValidatedResult<TIn, TOut>(@in, _other.Get, _subject.Get).Then();
}