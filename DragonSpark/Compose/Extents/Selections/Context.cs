using DragonSpark.Compose.Model;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime.Activation;
using DragonSpark.Runtime.Objects;
using System;
using System.Reflection;

namespace DragonSpark.Compose.Extents.Selections
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

		public AlterationSelector<T> Self => new AlterationSelector<T>(Self<T>.Default);

		public Selector<T, TypeInfo> Metadata => InstanceMetadata<T>.Default.Then();

		public Selector<T, Type> Type => InstanceType<T>.Default.Then();

		public Selector<T, T> Default() => DragonSpark.Model.Selection.Default<T>.Instance.Then();

		public Selector<T, TOut> Calling<TOut>(Func<T, TOut> select)
			=> new Selector<T, TOut>(select.Target as ISelect<T, TOut> ?? new Select<T, TOut>(select));

		public Selector<T, TOut> Calling<TOut>(Func<TOut> result) => new DelegatedResult<T, TOut>(result).Then();

		public AlterationSelector<T> Calling(Func<T, T> result) => new AlterationSelector<T>(new Alteration<T>(result));

		public Selector<T, TOut> Returning<TOut>(TOut result) => new FixedResult<T, TOut>(result).Then();

		public Selector<T, TOut> Returning<TOut>(IResult<TOut> result) => Calling(result.Get);

		public ConditionSelector<T> Returning(IResult<bool> condition) => Calling(condition.Get).Out().Then();

		public AlterationSelector<T> Returning(T result) => Calling(new FixedResult<T, T>(result).Get);

		public Selector<T, TOut> Default<TOut>() => Default<T, TOut>.Instance.Then();

		public Selector<T, TOut> Cast<TOut>() where TOut : T => CastOrDefault<T, TOut>.Default.Then();

		public Selector<T, Array<T>> Array() => Self.Result();

		public Selector<T, Func<TIn, TOut>> Delegate<TIn, TOut>(Func<T, Func<TIn, TOut>> select)
			=> new Select<T, Func<TIn, TOut>>(select).Then();

		public Selector<T, TOut> Activation<TOut>() => Activator<TOut>.Default.Then().Accept<T>();

		public Selector<T, TOut> StoredActivation<TOut>() where TOut : IActivateUsing<T> => Activations<T, TOut>.Default.Then();

		public Selector<T, TOut> Singleton<TOut>()
			=> Runtime.Activation.Singleton<TOut>.Default.Then().Accept<T>();

		public Selector<T, TOut> Instantiation<TOut>() => New<T, TOut>.Default.Then();
	}

	public sealed class Context<TIn, TOut>
	{
		public static Context<TIn, TOut> Instance { get; } = new Context<TIn, TOut>();

		Context() {}

		public Selector<TIn, TOut> Instantiation => New<TIn, TOut>.Default.Then();

		public Cast<TIn, TOut> Cast => Cast<TIn, TOut>.Default;

		public Selector<TIn, TOut> Activation() => Activator<TOut>.Default.Then().Accept<TIn>().Return().Then();

		public Selector<TIn, TOut> Singleton() => Singleton<TOut>.Default.Then().Accept<TIn>().Return().Then();

		public Selector<TIn, TOut> Returning(TOut result) => new FixedResult<TIn, TOut>(result).Then();

		public Selector<TIn, TOut> Returning(Func<TOut> result) => new DelegatedResult<TIn, TOut>(result).Then();
	}
}