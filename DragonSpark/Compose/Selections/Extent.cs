using System;
using System.Reflection;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime;

namespace DragonSpark.Compose.Selections
{
	public sealed class Extent
	{
		public static Extent Default { get; } = new Extent();

		Extent() {}

		public SystemExtents System => SystemExtents.Instance;
		public Extent<object> Any => DefaultExtent<object>.Default;
		public Extent<None> None => DefaultExtent<None>.Default;

		public Extent<T> Type<T>() => DefaultExtent<T>.Default;

		public sealed class SystemExtents
		{
			public static SystemExtents Instance { get; } = new SystemExtents();

			SystemExtents() {}

			public Extent<Type> Type => DefaultExtent<Type>.Default;

			public Extent<TypeInfo> Metadata => DefaultExtent<TypeInfo>.Default;
		}
	}

	public class Extent<T>
	{
		protected Extent() {}

		public Selections As => Selections.Instance;

		public Context<T> By => Context<T>.Instance;

		public Extent<T, TOut> AndOf<TOut>() => DefaultExtent<T, TOut>.Default;

		public class Selections
		{
			public static Selections Instance { get; } = new Selections();

			Selections() {}

			public SequenceExtent<T> Sequence => SequenceExtent<T>.Default;
			public Extent<Func<T>> Delegate => DefaultExtent<Func<T>>.Default;
			public Extent<ICondition<T>> Condition => DefaultExtent<ICondition<T>>.Default;
			public Extent<IResult<T>> Result => DefaultExtent<IResult<T>>.Default;
			public Extent<ICommand<T>> Command => DefaultExtent<ICommand<T>>.Default;
		}
	}

	public class Extent<TIn, TOut>
	{
		protected Extent() {}

		public Selections As => Selections.Instance;

		public Context<TIn, TOut> By => Context<TIn, TOut>.Instance;

		public Into<TIn, TOut> Into => Into<TIn, TOut>.Default;

		public class Selections
		{
			public static Selections Instance { get; } = new Selections();

			Selections() {}

			public SequenceExtent<TIn, TOut> Sequence => SequenceExtent<TIn, TOut>.Default;
			public Extent<TIn, Func<TOut>> Delegate => DefaultExtent<TIn, Func<TOut>>.Default;
			public Extent<TIn, ICondition<TOut>> Condition => DefaultExtent<TIn, ICondition<TOut>>.Default;
			public Extent<TIn, IResult<TOut>> Result => DefaultExtent<TIn, IResult<TOut>>.Default;
			public Extent<TIn, ICommand<TOut>> Command => DefaultExtent<TIn, ICommand<TOut>>.Default;
		}
	}
}