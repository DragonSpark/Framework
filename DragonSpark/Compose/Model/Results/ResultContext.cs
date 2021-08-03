using DragonSpark.Compose.Model.Selection;
using DragonSpark.Compose.Model.Validation;
using DragonSpark.Model;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime.Objects;
using System;

namespace DragonSpark.Compose.Model.Results
{
	public class ResultContext<T>
	{
		public static implicit operator Func<T>(ResultContext<T> instance) => instance.Get().Get;

		readonly IResult<T> _instance;

		public ResultContext(IResult<T> instance) => _instance = instance;

		public Selector<TIn, T> Accept<TIn>() => new DelegatedResult<TIn, T>(this).Then();

		public Selector<None, T> Accept() => Accept<None>();

		public Selector<object, T> Any() => Accept<object>();

		public ResultContext<T> Singleton() => new DeferredSingleton<T>(_instance.Get).Then();

		public ResultContext<TOut> Select<TOut>(Selector<T, TOut> select) => Select(select.Get());

		public ResultContext<TOut> Select<TOut>(ISelect<T, TOut> select) => Select(select.Get);

		public ResultContext<TOut> Select<TOut>(Func<T, TOut> select)
			=> new DelegatedSelection<T, TOut>(select, this).Then();

		public ResultContext<TTo> Cast<TTo>() => Select(CastOrDefault<T, TTo>.Default);

		public ValidatedResultContext<T> Unless(ResultContext<T> other) => Unless(other.Get());

		public ValidatedResultContext<T> Unless(IResult<T> other) => new ValidatedResultContext<T>(Get(), other);

		public Func<T> ReferenceDelegate() => Delegates<T>.Default.Get(Get());

		public IResult<T> Get() => _instance;
	}
}