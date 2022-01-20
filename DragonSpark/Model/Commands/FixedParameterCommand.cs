using System;

namespace DragonSpark.Model.Commands;

public class FixedParameterCommand<T> : ICommand
{
	readonly Action<T> _command;
	readonly T         _parameter;

	public FixedParameterCommand(ICommand<T> command, T parameter) : this(command.Execute, parameter) {}

	public FixedParameterCommand(Action<T> command, T parameter)
	{
		_command   = command;
		_parameter = parameter;
	}

	public void Execute(None parameter)
	{
		_command(_parameter);
	}
}