using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime.Activation;
using DragonSpark.Runtime.Objects;
using System;
using System.Reflection;

namespace DragonSpark.Compose.Extents.Selections;

public sealed class SelectionContext
{
	public static SelectionContext Default { get; } = new();

	SelectionContext() : this(SelectionExtent.Default) {}

	public SelectionContext(SelectionExtent extent) => Of = extent;

	public SelectionExtent Of { get; }
}

public sealed class SelectionContext<T>
{
	public static SelectionContext<T> Instance { get; } = new();

	SelectionContext() {}

	public AlterationComposer<T> Self => new(Self<T>.Default);

	public Composer<T, TypeInfo> Metadata => InstanceMetadata<T>.Default.Then();

	public Composer<T, Type> Type => InstanceType<T>.Default.Then();

	public Composer<T, T> Default() => DragonSpark.Model.Selection.Default<T>.Instance.Then();

	public Composer<T, TOut> Calling<TOut>(Func<T, TOut> select)
		=> new(select.Target as ISelect<T, TOut> ?? new Select<T, TOut>(select));

	public Composer<T, TOut> Calling<TOut>(IResult<TOut> result) => Calling(result.Get);

	public Composer<T, TOut> Calling<TOut>(Func<TOut> result) => new DelegatedResult<T, TOut>(result).Then();

	public AlterationComposer<T> Calling(Func<T, T> result) => new(new Alteration<T>(result));

	public Composer<T, TOut> Returning<TOut>(TOut result) => new FixedResult<T, TOut>(result).Then();

	public Composer<T, TOut> Returning<TOut>(IResult<TOut> result) => Calling(result.Get);

	public ConditionComposer<T> Returning(IResult<bool> condition) => Calling(condition.Get).Out().Then();

	public AlterationComposer<T> Returning(T result) => Calling(new FixedResult<T, T>(result).Get);

	public Composer<T, TOut> Default<TOut>() => Default<T, TOut>.Instance.Then();

	public Composer<T, TOut> Cast<TOut>() where TOut : T => CastOrDefault<T, TOut>.Default.Then();

	public Composer<T, TOut> CastDown<TOut>() => CastOrDefault<T, TOut>.Default.Then();


	public Composer<T, Array<T>> Array() => Self.Yield().Result();

	public Composer<T, Func<TIn, TOut>> Delegate<TIn, TOut>(Func<T, Func<TIn, TOut>> select)
		=> new Select<T, Func<TIn, TOut>>(select).Then();

	public Composer<T, TOut> Activation<TOut>() => Activator<TOut>.Default.Then().Accept<T>();

	public Composer<T, TOut> StoredActivation<TOut>() where TOut : IActivateUsing<T>
		=> Activations<T, TOut>.Default.Then();

	public Composer<T, TOut> Singleton<TOut>()
		=> Runtime.Activation.Singleton<TOut>.Default.Then().Accept<T>();

	public Composer<T, TOut> Instantiation<TOut>() => New<T, TOut>.Default.Then();
}

public sealed class SelectionContext<TIn, TOut>
{
	public static SelectionContext<TIn, TOut> Instance { get; } = new();

	SelectionContext() {}

	public Composer<TIn, TOut> Instantiation => New<TIn, TOut>.Default.Then();

	public Cast<TIn, TOut> Cast => Cast<TIn, TOut>.Default;

	public Composer<TIn, TOut> Activation() => Activator<TOut>.Default.Then().Accept<TIn>().Return().Then();

	public Composer<TIn, TOut> Singleton() => Singleton<TOut>.Default.Then().Accept<TIn>().Return().Then();

	public Composer<TIn, TOut> Returning(TOut result) => new FixedResult<TIn, TOut>(result).Then();

	public Composer<TIn, TOut> Returning(Func<TOut> result) => new DelegatedResult<TIn, TOut>(result).Then();
}