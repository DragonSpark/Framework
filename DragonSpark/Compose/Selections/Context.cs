using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Query;
using DragonSpark.Runtime.Activation;
using DragonSpark.Runtime.Objects;
using System;
using System.Reflection;

namespace DragonSpark.Compose.Selections
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

		public IAlteration<T> Self => Self<T>.Default;

		public ISelect<T, TypeInfo> Metadata => InstanceMetadata<T>.Default;
		public ISelect<T, Type> Type => InstanceType<T>.Default;

		public ISelect<T, T> Default() => Model.Selection.Default<T>.Instance;

		public ISelect<T, TOut> Calling<TOut>(Func<T, TOut> select) => new Model.Selection.Select<T, TOut>(select);

		public ISelect<T, TOut> Calling<TOut>(Func<TOut> result) => new DelegatedResult<T, TOut>(result);

		public IAlteration<T> Calling(Func<T, T> result) => new Alteration<T>(result);

		public ISelect<T, TOut> Returning<TOut>(TOut result) => new FixedResult<T, TOut>(result);

		public ISelect<T, TOut> Returning<TOut>(IResult<TOut> result) => Calling(result.Get);

		public ICondition<T> Returning(IResult<bool> condition) => Calling(condition.Get).ToCondition();

		public IAlteration<T> Returning(T result) => Calling(new FixedResult<T, T>(result).Get);

		public ISelect<T, TOut> Default<TOut>() => Default<T, TOut>.Instance;

		public ISelect<T, TOut> Cast<TOut>() where TOut : T => CastOrDefault<T, TOut>.Default;

		public ISelect<T, Array<T>> Array() => Self.Select(Yield<T>.Default).Result();

		public ISelect<T, Func<TIn, TOut>> Delegate<TIn, TOut>(Func<T, Func<TIn, TOut>> select)
			=> new Model.Selection.Select<T, Func<TIn, TOut>>(select);

		public ISelect<T, TOut> Activation<TOut>() => Activator<TOut>.Default.ToSelect(Start.An.Extent<T>());

		public ISelect<T, TOut> StoredActivation<TOut>() where TOut : IActivateUsing<T> => Activations<T, TOut>.Default;

		public ISelect<T, TOut> Singleton<TOut>()
			=> Runtime.Activation.Singleton<TOut>.Default.ToSelect(Start.An.Extent<T>());

		public ISelect<T, TOut> Instantiation<TOut>() => New<T, TOut>.Default;

		/*public Location<T, TOut> Location<TOut>() => Location<T, TOut>.Default;*/
	}

	public sealed class Context<TIn, TOut>
	{
		public static Context<TIn, TOut> Instance { get; } = new Context<TIn, TOut>();

		Context() {}

		public ISelect<TIn, TOut> Instantiation => New<TIn, TOut>.Default;

		/*public Location<TIn, TOut> Location => Location<TIn, TOut>.Default;*/

		public Cast<TIn, TOut> Cast => Cast<TIn, TOut>.Default;

		public ISelect<TIn, TOut> Activation() => Activator<TOut>.Default.ToSelect(Start.An.Extent<TIn>());

		public ISelect<TIn, TOut> Singleton() => Singleton<TOut>.Default.ToSelect(Start.An.Extent<TIn>());

		public ISelect<TIn, TOut> Returning(TOut result) => new FixedResult<TIn, TOut>(result);

		public ISelect<TIn, TOut> Returning(Func<TOut> result) => new DelegatedResult<TIn, TOut>(result);
	}
}