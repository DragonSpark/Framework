﻿using DragonSpark.Compose.Model.Operations;
using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Compose.Model.Commands;

public class CommandContext : CommandContext<None>
{
	public static implicit operator Action(CommandContext instance) => instance.Get().Execute;

	public CommandContext(ICommand command) : base(command) => Command = command;

	public ICommand Command { get; }

	public CommandContext<object> Any() => new(new Any(Command));

	public CommandContext<T> Accept<T>() => new(new Accept<T>(Command));

	public new OperationSelector Operation() => new(new CommandOperation(Get().Execute));
}

public class CommandContext<T> : Instance<ICommand<T>>
{
	public static implicit operator System.Action<T>(CommandContext<T> instance) => instance.Get().Execute;

	public CommandContext(ICommand<T> command) : base(command) {}

	public CommandContext Bind(T? parameter = default)
		=> new(new FixedParameterCommand<T>(Get().Execute, parameter!));

	public CommandContext Bind(IResult<T> parameter) => Bind(parameter.Get);

	public CommandContext Bind(Func<T> parameter)
		=> new(new DelegatedParameterCommand<T>(Get().Execute, parameter));

	public CommandContext<T> Prepend(ICommand<T> command)
		=> new(new AppendedCommand<T>(command, Get()));

	public CommandContext<T> Prepend(params ICommand<T>[] commands)
		=> new(new Commands<T>(commands.Append(Get()).Result()));

	public CommandContext<T> Append(System.Action<T> command) => Append(Start.A.Command(command).Get());

	public CommandContext<T> Append(ICommand<T> command) => new(new AppendedCommand<T>(Get(), command));

	public CommandContext<T> Append(params ICommand<T>[] commands)
		=> new(new Commands<T>(commands.Prepend(Get()).Result()));


	public Selector<T, None> Selection() => new(new Action<T>(this));

	public OperationContext<T> Operation() => new(new CommandOperation<T>(Get().Execute));

	public AlterationSelector<T> ToConfiguration() => new(new DragonSpark.Model.Selection.Alterations.Configured<T>(Get().Execute));
}