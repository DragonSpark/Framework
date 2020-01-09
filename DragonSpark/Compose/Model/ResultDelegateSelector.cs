using DragonSpark.Model;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime;
using DragonSpark.Runtime.Invocation;
using DragonSpark.Runtime.Objects;
using System;

namespace DragonSpark.Compose.Model
{
	public class ResultDelegateSelector<_, T> : Selector<_, Func<T>>
	{
		public ResultDelegateSelector(ISelect<_, Func<T>> subject) : base(subject) {}

		public Selector<_, Func<T>> Singleton() => Select(SingletonDelegate<T>.Default);

		public Selector<_, T> Invoke() => Select(Call<T>.Default);
	}

	public sealed class SelectionDelegateResultContext<TIn, TOut> : ResultContext<Func<TIn, TOut>>
	{
		public SelectionDelegateResultContext(IResult<Func<TIn, TOut>> instance) : base(instance) {}

		public Selector<TIn, TOut> Assume() => new DelegatedAssume<TIn, TOut>(Get().Get).Then();
	}

	public sealed class SelectionResultContext<TIn, TOut> : ResultContext<ISelect<TIn, TOut>>
	{
		public SelectionResultContext(IResult<ISelect<TIn, TOut>> instance) : base(instance) {}

		public Selector<TIn, TOut> Assume() => new Assume<TIn, TOut>(Get()).Then();
	}

	public sealed class ValidatedResultContext<T>
	{
		readonly IResult<T> _subject;
		readonly IResult<T> _other;

		public ValidatedResultContext(IResult<T> subject, IResult<T> other)
		{
			_subject = subject;
			_other   = other;
		}

		public ResultContext<T> IsAssigned() => Is(IsAssigned<T>.Default);

		public ResultContext<T> Is(Func<T, bool> condition) => Is(Start.A.Condition(condition).Out());

		public ResultContext<T> Is(ICondition<T> condition)
			=> new ValidatedResult<T>(condition, _other, _subject).Then();

		public ResultContext<T> Is(ICondition condition) => Is(condition.Get);

		public ResultContext<T> Is(Func<bool> condition)
			=> new Validated<T>(condition, _other.Get, _subject.Get).Then();
	}

	public class ResultContext<T>
	{
		readonly IResult<T> _instance;

		public static implicit operator Func<T>(ResultContext<T> instance) => instance.Get().Get;

		public ResultContext(IResult<T> instance) => _instance = instance;

		public Selector<TIn, T> Accept<TIn>() => new DelegatedResult<TIn, T>(this).Then();

		public Selector<None, T> Accept() => Accept<None>();

		public ResultContext<T> Singleton() => new DeferredSingleton<T>(this).Then();

		public ResultContext<TOut> Select<TOut>(Selector<T, TOut> select) => Select(select.Get());

		public ResultContext<TOut> Select<TOut>(ISelect<T, TOut> select) => Select(select.Get);

		public ResultContext<TOut> Select<TOut>(Func<T, TOut> select)
			=> new DelegatedSelection<T, TOut>(select, this).Then();

		public ResultContext<TTo> Cast<TTo>() => Select(CastOrDefault<T, TTo>.Default);

		public ValidatedResultContext<T> Unless(ResultContext<T> other) => Unless(other.Get());

		public ValidatedResultContext<T> Unless(IResult<T> other) => new ValidatedResultContext<T>(Get(), other);

		public Func<T> ReferenceDelegate() => DragonSpark.Model.Results.Delegates<T>.Default.Get(Get());

		public IResult<T> Get() => _instance;
	}
}