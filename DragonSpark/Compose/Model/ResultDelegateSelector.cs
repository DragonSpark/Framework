using DragonSpark.Model;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
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

	public class ResultContext<T> // : Context<IResult<T>>
	{
		readonly IResult<T> _instance;

		public static implicit operator Func<T>(ResultContext<T> instance) => instance.Get().Get;

		public ResultContext(IResult<T> instance)// : base(instance)
			=> _instance = instance;

		public Selector<TIn, T> Accept<TIn>() => new DelegatedResult<TIn, T>(this).Then();

		public Selector<None, T> Accept() => Accept<None>();

		public ResultContext<T> Singleton() => new DeferredSingleton<T>(this).Then();

		public ResultContext<TOut> Select<TOut>(ISelect<T, TOut> select) => Select(select.Get);

		public ResultContext<TOut> Select<TOut>(Func<T, TOut> select)
			=> new DelegatedSelection<T, TOut>(select, this).Then();

		public ResultContext<TTo> Cast<TTo>() => Select(CastOrDefault<T, TTo>.Default);

		public IResult<T> Get() => _instance;
	}

	public class Context<T> : Instance<T>
	{
		public Context(T instance) : base(instance) {}
	}
}