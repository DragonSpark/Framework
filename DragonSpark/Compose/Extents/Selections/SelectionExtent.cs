using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;
using System;
using System.Reflection;

namespace DragonSpark.Compose.Extents.Selections
{
	public sealed class SelectionExtent
	{
		public static SelectionExtent Default { get; } = new SelectionExtent();

		SelectionExtent() {}

		public SystemExtents System => SystemExtents.Instance;
		public SelectionExtent<object> Any => DefaultSelectionExtent<object>.Default;
		public SelectionExtent<None> None => DefaultSelectionExtent<None>.Default;

		public SelectionExtent<T> Type<T>() => DefaultSelectionExtent<T>.Default;

		public sealed class SystemExtents
		{
			public static SystemExtents Instance { get; } = new SystemExtents();

			SystemExtents() {}

			public SelectionExtent<Type> Type => DefaultSelectionExtent<Type>.Default;

			public SelectionExtent<TypeInfo> Metadata => DefaultSelectionExtent<TypeInfo>.Default;
		}
	}

	public class SelectionExtent<T>
	{
		protected SelectionExtent() {}

		public Selections As => Selections.Instance;

		public SelectionContext<T> By => SelectionContext<T>.Instance;

		public SelectionExtent<T, TOut> AndOf<TOut>() => DefaultSelectionExtent<T, TOut>.Default;

		public class Selections
		{
			public static Selections Instance { get; } = new Selections();

			Selections() {}

			public SequenceExtent<T> Sequence => SequenceExtent<T>.Default;
			public SelectionExtent<Func<T>> Delegate => DefaultSelectionExtent<Func<T>>.Default;
			public SelectionExtent<ICondition<T>> Condition => DefaultSelectionExtent<ICondition<T>>.Default;
			public SelectionExtent<IResult<T>> Result => DefaultSelectionExtent<IResult<T>>.Default;
			public SelectionExtent<ICommand<T>> Command => DefaultSelectionExtent<ICommand<T>>.Default;
		}
	}

	public class SelectionExtent<TIn, TOut>
	{
		protected SelectionExtent() {}

		public Selections As => Selections.Instance;

		public SelectionContext<TIn, TOut> By => SelectionContext<TIn, TOut>.Instance;

		public Into<TIn, TOut> Into => Into<TIn, TOut>.Default;

		public class Selections
		{
			public static Selections Instance { get; } = new Selections();

			Selections() {}

			public SequenceExtent<TIn, TOut> Sequence => SequenceExtent<TIn, TOut>.Default;
			public SelectionExtent<TIn, Func<TOut>> Delegate => DefaultSelectionExtent<TIn, Func<TOut>>.Default;

			public SelectionExtent<TIn, ICondition<TOut>> Condition
				=> DefaultSelectionExtent<TIn, ICondition<TOut>>.Default;

			public SelectionExtent<TIn, IResult<TOut>> Result => DefaultSelectionExtent<TIn, IResult<TOut>>.Default;
			public SelectionExtent<TIn, ICommand<TOut>> Command => DefaultSelectionExtent<TIn, ICommand<TOut>>.Default;
		}
	}
}