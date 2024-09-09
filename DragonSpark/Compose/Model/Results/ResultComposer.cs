using DragonSpark.Compose.Model.Operations;
using DragonSpark.Compose.Model.Selection;
using DragonSpark.Compose.Model.Validation;
using DragonSpark.Model;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime.Objects;
using System;

namespace DragonSpark.Compose.Model.Results;

public class ResultComposer<T>
{
	public static implicit operator Func<T>(ResultComposer<T> instance) => instance.Get().Get;

	readonly IResult<T> _instance;

	public ResultComposer(IResult<T> instance) => _instance = instance;

	public Composer<TIn, T> Accept<TIn>() => new DelegatedResult<TIn, T>(this).Then();

	public Composer<None, T> Accept() => Accept<None>();

	public Composer<object, T> Any() => Accept<object>();

	public OperationResultComposer<T> Operation() => new (_instance.Then().Select(x => x.ToOperation()).Out());

	public ResultComposer<T> Singleton() => new Deferred<T>(_instance.Get).Then();

	public ResultComposer<TOut> Select<TOut>(Composer<T, TOut> select) => Select(select.Get());

	public ResultComposer<TOut> Select<TOut>(ISelect<T, TOut> select) => Select(select.Get);

	public ResultComposer<TOut> Select<TOut>(Func<T, TOut> select) => new SelectedResult<T, TOut>(this, select).Then();

	public ResultComposer<TTo> Cast<TTo>() => Select(CastOrDefault<T, TTo>.Default);

	public ValidatedResultComposer<T> Unless(ResultComposer<T> other) => Unless(other.Get());

	public ValidatedResultComposer<T> Unless(IResult<T> other) => new(Get(), other);

	public Func<T> ReferenceDelegate() => Delegates<T>.Default.Get(Get());

	public IResult<T> Get() => _instance;
}