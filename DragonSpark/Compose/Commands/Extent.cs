using System;
using System.Reflection;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime;

namespace DragonSpark.Compose.Commands
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
}