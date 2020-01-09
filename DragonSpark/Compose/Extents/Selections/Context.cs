﻿using DragonSpark.Compose.Model;
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

		public IAlteration<T> Self => Self<T>.Default;

		public Selector<T, TypeInfo> Metadata => InstanceMetadata<T>.Default.Then();
		public ISelect<T, Type> Type => InstanceType<T>.Default;

		public ISelect<T, T> Default() => DragonSpark.Model.Selection.Default<T>.Instance;

		public ISelect<T, TOut> Calling<TOut>(Func<T, TOut> select)
			=> select.Target as ISelect<T, TOut> ?? new DragonSpark.Model.Selection.Select<T, TOut>(select);

		public ISelect<T, TOut> Calling<TOut>(Func<TOut> result) => new DelegatedResult<T, TOut>(result);

		public IAlteration<T> Calling(Func<T, T> result) => new Alteration<T>(result);

		public ISelect<T, TOut> Returning<TOut>(TOut result) => new FixedResult<T, TOut>(result);

		public ISelect<T, TOut> Returning<TOut>(IResult<TOut> result) => Calling(result.Get);

		public ICondition<T> Returning(IResult<bool> condition) => Calling(condition.Get).Then().Out();

		public IAlteration<T> Returning(T result) => Calling(new FixedResult<T, T>(result).Get);

		public ISelect<T, TOut> Default<TOut>() => Default<T, TOut>.Instance;

		public ISelect<T, TOut> Cast<TOut>() where TOut : T => CastOrDefault<T, TOut>.Default;

		public ISelect<T, Array<T>> Array() => Self.Select(Yield<T>.Default).Result();

		public ISelect<T, Func<TIn, TOut>> Delegate<TIn, TOut>(Func<T, Func<TIn, TOut>> select)
			=> new DragonSpark.Model.Selection.Select<T, Func<TIn, TOut>>(select);

		public ISelect<T, TOut> Activation<TOut>() => Activator<TOut>.Default.Then().Accept<T>().Return();

		public ISelect<T, TOut> StoredActivation<TOut>() where TOut : IActivateUsing<T> => Activations<T, TOut>.Default;

		public ISelect<T, TOut> Singleton<TOut>()
			=> Runtime.Activation.Singleton<TOut>.Default.Then().Accept<T>().Return();

		public ISelect<T, TOut> Instantiation<TOut>() => New<T, TOut>.Default;
	}

	public sealed class Context<TIn, TOut>
	{
		public static Context<TIn, TOut> Instance { get; } = new Context<TIn, TOut>();

		Context() {}

		public ISelect<TIn, TOut> Instantiation => New<TIn, TOut>.Default;

		public Cast<TIn, TOut> Cast => Cast<TIn, TOut>.Default;

		public ISelect<TIn, TOut> Activation() => Activator<TOut>.Default.Then().Accept<TIn>().Return();

		public ISelect<TIn, TOut> Singleton() => Singleton<TOut>.Default.Then().Accept<TIn>().Return();

		public ISelect<TIn, TOut> Returning(TOut result) => new FixedResult<TIn, TOut>(result);

		public ISelect<TIn, TOut> Returning(Func<TOut> result) => new DelegatedResult<TIn, TOut>(result);
	}
}