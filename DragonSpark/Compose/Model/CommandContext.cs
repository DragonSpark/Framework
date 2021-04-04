﻿using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Compose.Model
{
	public class CommandContext : CommandContext<None>
	{
		public static implicit operator System.Action(CommandContext instance) => instance.Get().Execute;

		public CommandContext(ICommand command) : base(command) => Command = command;

		public ICommand Command { get; }

		public CommandContext<object> Any() => new CommandContext<object>(new Any(Command));

		public CommandContext<T> Accept<T>() => new CommandContext<T>(new Accept<T>(Command));

		public new OperationSelector Operation() => new OperationSelector(new CommandOperation(Get().Execute));
	}

	public class CommandContext<T> : DragonSpark.Model.Results.Instance<ICommand<T>>
	{
		public static implicit operator System.Action<T>(CommandContext<T> instance) => instance.Get().Execute;

		public CommandContext(ICommand<T> command) : base(command) {}

		public CommandContext Bind(T? parameter = default)
			=> new CommandContext(new FixedParameterCommand<T>(Get().Execute, parameter!));

		public CommandContext Bind(IResult<T> parameter)
			=> new CommandContext(new DelegatedParameterCommand<T>(Get().Execute, parameter.Get));

		public CommandContext<T> Prepend(params ICommand<T>[] commands)
			=> new CommandContext<T>(new CompositeCommand<T>(commands.Append(Get()).Result()));

		public CommandContext<T> Append(System.Action<T> command) => Append(Start.A.Command(command).Get());

		public CommandContext<T> Append(ICommand<T> command) => new(new AppendedCommand<T>(Get(), command));

		public CommandContext<T> Append(params ICommand<T>[] commands)
			=> new CommandContext<T>(new CompositeCommand<T>(commands.Prepend(Get()).Result()));

		public CommandContext<DragonSpark.Model.Sequences.Store<T>> Many()
			=> new CommandContext<DragonSpark.Model.Sequences.Store<T>>(new ManyCommand<T>(Get()));

		public Selector<T, None> Selection() => new Selector<T, None>(new Action<T>(this));

		public OperationContext<T> Operation() => new OperationContext<T>(new CommandOperation<T>(Get().Execute));

		public AlterationSelector<T> ToConfiguration() => new AlterationSelector<T>(new Configured<T>(Get().Execute));
	}
}