using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Reflection.Types;
using DragonSpark.Runtime.Objects;
using System;

namespace DragonSpark.Compose.Model
{
	public sealed class UnlessUsingContext<TIn, TOut>
	{
		readonly ISelect<TIn, TOut> _subject;
		readonly ISelect<TIn, TOut> _other;

		public UnlessUsingContext(ISelect<TIn, TOut> subject, ISelect<TIn, TOut> other)
		{
			_subject = subject;
			_other   = other;
		}

		public Selector<TIn, TOut> IsOf<T>() => Results(IsOf<TOut, T>.Default.Get);

		public Selector<TIn, TOut> ResultsInAssigned() => Results(Is.Assigned<TOut>().Get);

		public Selector<TIn, TOut> Results(Func<TOut, bool> @in)
			=> new ValidatedResult<TIn, TOut>(@in, _other.Get, _subject.Get).Then();
	}

	public sealed class UnlessResultContext<TIn, TOut>
	{
		readonly ISelect<TIn, TOut> _subject;
		readonly Func<TIn, bool>    _condition;

		public UnlessResultContext(ISelect<TIn, TOut> subject, Func<TIn, bool> condition)
		{
			_subject   = subject;
			_condition = condition;
		}

		public ConditionalSelector<TIn, TOut> ThenUse(ISelect<TIn, TOut> instead) => ThenUse(instead.Get);

		public ConditionalSelector<TIn, TOut> ThenUse(Func<TIn, TOut> instead)
			=> new Conditional<TIn, TOut>(_condition, instead, _subject.Get).Then();
	}

	public sealed class UnlessInputContext<TIn, TOut>
	{
		readonly ISelect<TIn, TOut> _subject;

		public UnlessInputContext(ISelect<TIn, TOut> subject) => _subject = subject;

		public Selector<TIn, TOut> IsOf<TOther>(ISelect<TOther, TOut> other) => IsOf(other.ToDelegate());

		public Selector<TIn, TOut> IsOf<TOther>(Func<TOther, TOut> other)
			=> IsOf<TOther>().ThenUse(CastOrThrow<TIn, TOther>.Default.Select(other));

		public UnlessResultContext<TIn, TOut> IsOf<T>() => Is(IsOf<TIn, T>.Default);

		public UnlessResultContext<TIn, TOut> IsUnassigned() => Is(Compose.Is.Assigned<TIn>().Then().Inverse());

		public UnlessResultContext<TIn, TOut> Is(ISelect<TIn, bool> condition) => Is(condition.Get);

		public UnlessResultContext<TIn, TOut> Is(Func<TIn, bool> condition)
			=> new UnlessResultContext<TIn, TOut>(_subject, condition);
	}

	public sealed class UnlessContext<TIn, TOut>
	{
		readonly ISelect<TIn, TOut> _subject;

		public UnlessContext(ISelect<TIn, TOut> subject) => _subject = subject;

		public UnlessInputContext<TIn, TOut> Input => new UnlessInputContext<TIn, TOut>(_subject);

		public UnlessUsingContext<TIn, TOut> Using(ISelect<TIn, TOut> instead)
			=> new UnlessUsingContext<TIn, TOut>(_subject, instead);

		public ConditionalSelector<TIn, TTo> UsingWhen<TTo>(IConditional<TOut, TTo> select)
			=> new Conditional<TIn, TTo>(_subject.Select(select.Condition).Get, _subject.Select(select).Get).Then();
	}

}
