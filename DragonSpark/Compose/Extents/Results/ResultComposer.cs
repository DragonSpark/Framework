using DragonSpark.Model;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime.Activation;
using System;

namespace DragonSpark.Compose.Extents.Results;

public sealed class ResultComposer
{
	public static ResultComposer Default { get; } = new();

	ResultComposer() : this(ResultExtent.Default) {}

	public ResultComposer(ResultExtent resultExtent) => Of = resultExtent;

	public ResultExtent Of { get; }
}

public sealed class ResultComposer<T>
{
	public static ResultComposer<T> Instance { get; } = new();

	ResultComposer() {}

	public Model.Results.ResultComposer<T> Activation() => Activator<T>.Default.Then();

	public Model.Results.ResultComposer<T> Singleton() => Singleton<T>.Default.Then();

	public Model.Results.ResultComposer<T> Instantiation() => New<T>.Default.Then();

	public Model.Results.ResultComposer<T> Default() => DragonSpark.Model.Results.Default<T>.Instance.Then();

	public Model.Results.ResultComposer<T> Using(T instance) => new DragonSpark.Model.Results.Instance<T>(instance).Then();

	public Model.Results.ResultComposer<T> Using(ISelect<None, T> source) => new Result<T>(source.Get).Then();

	public Model.Results.ResultComposer<T> Using(IResult<T> instance) => instance.Then();

	public Model.Results.ResultComposer<T> Using<TResult>() where TResult : class, IResult<T>
		=> Activator<TResult>.Default.Get().To(Using);

	public Model.Results.ResultComposer<T> Calling(Func<T> select)
		=> new(select.Target as IResult<T> ?? new Result<T>(select));
}