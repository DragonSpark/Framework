using DragonSpark.Compose.Model.Operations;
using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Compose.Model.Commands;

public class CommandComposer : CommandComposer<None>
{
	public static implicit operator Action(CommandComposer instance) => instance.Get().Execute;

	public CommandComposer(ICommand command) : base(command) => Command = command;

	public ICommand Command { get; }

	public CommandComposer<object> Any() => new(new Any(Command));

	public CommandComposer<T> Accept<T>() => new(new Accept<T>(Command));

	public new OperationComposer Operation() => new(new CommandOperation(Get().Execute));
}

public class CommandComposer<T> : Instance<ICommand<T>>
{
	public static implicit operator System.Action<T>(CommandComposer<T> instance) => instance.Get().Execute;

	public CommandComposer(ICommand<T> command) : base(command) {}

	public CommandComposer Bind(T? parameter = default)
		=> new(new FixedParameterCommand<T>(Get().Execute, parameter!));

	public CommandComposer Bind(IResult<T> parameter) => Bind(parameter.Get);

	public CommandComposer Bind(Func<T> parameter)
		=> new(new DelegatedParameterCommand<T>(Get().Execute, parameter));

	public CommandComposer<T> Prepend(ICommand<T> command)
		=> new(new AppendedCommand<T>(command, Get()));

	public CommandComposer<T> Prepend(params ICommand<T>[] commands)
		=> new(new Commands<T>(commands.Append(Get()).Result()));

	public CommandComposer<T> Append(System.Action<T> command) => Append(Start.A.Command(command).Get());

	public CommandComposer<T> Append(ICommand<T> command) => new(new AppendedCommand<T>(Get(), command));

	public CommandComposer<T> Append(params ICommand<T>[] commands)
		=> new(new Commands<T>(commands.Prepend(Get()).Result()));


	public Composer<T, None> Selection() => new(new Action<T>(this));

	public OperationComposer<T> Operation() => new(new CommandOperation<T>(Get().Execute));

	public AlterationComposer<T> ToConfiguration() => new(new DragonSpark.Model.Selection.Alterations.Configured<T>(Get().Execute));
}