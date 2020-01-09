using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;
using System;
using System.Reflection;

namespace DragonSpark.Compose.Extents.Results
{
	public sealed class ResultExtent
	{
		public static ResultExtent Default { get; } = new ResultExtent();

		ResultExtent() {}

		public SystemExtents System => SystemExtents.Instance;

		public ResultExtent<object> Any => DefaultExtent<object>.Default;

		public ResultExtent<None> None => DefaultExtent<None>.Default;

		public ResultExtent<T> Type<T>() => DefaultExtent<T>.Default;

		public sealed class SystemExtents
		{
			public static SystemExtents Instance { get; } = new SystemExtents();

			SystemExtents() {}

			public ResultExtent<Type> Type => DefaultExtent<Type>.Default;

			public ResultExtent<TypeInfo> Metadata => DefaultExtent<TypeInfo>.Default;
		}
	}

	public class ResultExtent<T>
	{
		protected ResultExtent() {}

		public Selections As => Selections.Instance;

		public ResultContext<T> By => ResultContext<T>.Instance;

		public class Selections
		{
			public static Selections Instance { get; } = new Selections();

			Selections() {}

			public SequenceResultExtent<T> Sequence => SequenceResultExtent<T>.Default;
			public ResultExtent<Func<T>> Delegate => DefaultExtent<Func<T>>.Default;
			public ResultExtent<ICondition<T>> Condition => DefaultExtent<ICondition<T>>.Default;
			public ResultExtent<IResult<T>> Result => DefaultExtent<IResult<T>>.Default;
			public ResultExtent<ICommand<T>> Command => DefaultExtent<ICommand<T>>.Default;
		}
	}
}