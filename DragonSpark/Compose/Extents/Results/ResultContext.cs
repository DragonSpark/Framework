using DragonSpark.Model;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime.Activation;
using System;

namespace DragonSpark.Compose.Extents.Results;

public sealed class ResultContext
{
	public static ResultContext Default { get; } = new ResultContext();

	ResultContext() : this(ResultExtent.Default) {}

	public ResultContext(ResultExtent resultExtent) => Of = resultExtent;

	public ResultExtent Of { get; }
}

public sealed class ResultContext<T>
{
	public static ResultContext<T> Instance { get; } = new ResultContext<T>();

	ResultContext() {}

	public Model.Results.ResultContext<T> Activation() => Activator<T>.Default.Then();

	public Model.Results.ResultContext<T> Singleton() => Singleton<T>.Default.Then();

	public Model.Results.ResultContext<T> Instantiation() => New<T>.Default.Then();

	public Model.Results.ResultContext<T> Default() => DragonSpark.Model.Results.Default<T>.Instance.Then();

	public Model.Results.ResultContext<T> Using(T instance) => new DragonSpark.Model.Results.Instance<T>(instance).Then();

	public Model.Results.ResultContext<T> Using(ISelect<None, T> source) => new Result<T>(source.Get).Then();

	public Model.Results.ResultContext<T> Using(IResult<T> instance) => instance.Then();

	public Model.Results.ResultContext<T> Using<TResult>() where TResult : class, IResult<T>
		=> Activator<TResult>.Default.Get().To(Using);

	public Model.Results.ResultContext<T> Calling(Func<T> select)
		=> new Model.Results.ResultContext<T>(select.Target as IResult<T> ?? new Result<T>(select));
}