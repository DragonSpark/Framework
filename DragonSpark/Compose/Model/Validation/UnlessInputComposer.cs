using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Types;
using DragonSpark.Runtime.Objects;
using System;

namespace DragonSpark.Compose.Model.Validation;

public sealed class UnlessInputComposer<TIn, TOut>
{
	readonly ISelect<TIn, TOut> _subject;

	public UnlessInputComposer(ISelect<TIn, TOut> subject) => _subject = subject;

	public Composer<TIn, TOut> IsOf<TOther>(ISelect<TOther, TOut> other) => IsOf(other.ToDelegate());

	public Composer<TIn, TOut> IsOf<TOther>(Func<TOther, TOut> other)
		=> IsOf<TOther>().ThenUse(CastOrThrow<TIn, TOther>.Default.Select(other));

	public UnlessResultComposer<TIn, TOut> IsOf<T>() => Is(IsOf<TIn, T>.Default);

	public UnlessResultComposer<TIn, TOut> IsUnassigned() => Is(Compose.Is.Assigned<TIn>().Inverse());

	public UnlessResultComposer<TIn, TOut> Is(ISelect<TIn, bool> condition) => Is(condition.Get);

	public UnlessResultComposer<TIn, TOut> Is(Func<TIn, bool> condition) => new(_subject, condition);
}