using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;
using System;
using System.Reflection;

namespace DragonSpark.Compose.Extents.Commands
{
	public sealed class CommandExtent
	{
		public static CommandExtent Default { get; } = new CommandExtent();

		CommandExtent() {}

		public SystemExtents System => SystemExtents.Instance;

		public CommandExtent<object> Any => DefaultCommandExtent<object>.Default;

		public CommandExtent<None> None => DefaultCommandExtent<None>.Default;

		public CommandExtent<T> Type<T>() => DefaultCommandExtent<T>.Default;

		public sealed class SystemExtents
		{
			public static SystemExtents Instance { get; } = new SystemExtents();

			SystemExtents() {}

			public CommandExtent<Type> Type => DefaultCommandExtent<Type>.Default;

			public CommandExtent<TypeInfo> Metadata => DefaultCommandExtent<TypeInfo>.Default;
		}
	}

	public class CommandExtent<T>
	{
		protected CommandExtent() {}

		public Selections As => Selections.Instance;

		public CommandContext<T> By => CommandContext<T>.Instance;

		public class Selections
		{
			public static Selections Instance { get; } = new Selections();

			Selections() {}

			public SequenceCommandExtent<T> Sequence => SequenceCommandExtent<T>.Default;
			public CommandExtent<Func<T>> Delegate => DefaultCommandExtent<Func<T>>.Default;
			public CommandExtent<ICondition<T>> Condition => DefaultCommandExtent<ICondition<T>>.Default;
			public CommandExtent<IResult<T>> Result => DefaultCommandExtent<IResult<T>>.Default;
			public CommandExtent<ICommand<T>> Command => DefaultCommandExtent<ICommand<T>>.Default;
		}
	}
}