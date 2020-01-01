using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime;
using DragonSpark.Runtime.Activation;
using System;

namespace DragonSpark.Compose.Results
{
	public sealed class Context
	{
		public static Context Default { get; } = new Context();

		Context() : this(Extent.Default) {}

		public Context(Extent extent) => Of = extent;

		public Extent Of { get; }
	}

	public sealed class Context<T>
	{
		public static Context<T> Instance { get; } = new Context<T>();

		Context() {}

		/*public Location<T> Location => Location<T>.Default;*/

		public IResult<T> Activation() => Activator<T>.Default;

		public IResult<T> Singleton() => Singleton<T>.Default;

		public IResult<T> Instantiation() => New<T>.Default;

		public IResult<T> Default() => Model.Results.Default<T>.Instance;

		public IResult<T> Using(T instance) => new Instance<T>(instance);

		public IResult<T> Using(ISelect<None, T> source) => new Result<T>(source.Get);

		public IResult<T> Using(IResult<T> instance) => instance;

		public IResult<T> Using<TResult>() where TResult : class, IResult<T>
			=> Activator<TResult>.Default.Get().To(Using);

		public IResult<T> Calling(Func<T> select) => new Result<T>(select);
	}
}