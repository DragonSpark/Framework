using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Types;
using DragonSpark.Runtime.Objects;
using System;

namespace DragonSpark.Compose.Model.Validation;

public sealed class UnlessInputContext<TIn, TOut>
{
	readonly ISelect<TIn, TOut> _subject;

	public UnlessInputContext(ISelect<TIn, TOut> subject) => _subject = subject;

	public Selector<TIn, TOut> IsOf<TOther>(ISelect<TOther, TOut> other) => IsOf(other.ToDelegate());

	public Selector<TIn, TOut> IsOf<TOther>(Func<TOther, TOut> other)
		=> IsOf<TOther>().ThenUse(CastOrThrow<TIn, TOther>.Default.Select(other));

	public UnlessResultContext<TIn, TOut> IsOf<T>() => Is(IsOf<TIn, T>.Default);

	public UnlessResultContext<TIn, TOut> IsUnassigned() => Is(Compose.Is.Assigned<TIn>().Inverse());

	public UnlessResultContext<TIn, TOut> Is(ISelect<TIn, bool> condition) => Is(condition.Get);

	public UnlessResultContext<TIn, TOut> Is(Func<TIn, bool> condition)
		=> new UnlessResultContext<TIn, TOut>(_subject, condition);
}