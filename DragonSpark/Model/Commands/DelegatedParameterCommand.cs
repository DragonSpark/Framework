using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Model.Commands
{
	public class DelegatedParameterCommand<T> : ICommand
	{
		readonly Action<T> _command;
		readonly Func<T>   _parameter;

		public DelegatedParameterCommand(ICommand<T> command, IResult<T> parameter)
			: this(command.Execute, parameter.Get) {}

		public DelegatedParameterCommand(Action<T> command, Func<T> parameter)
		{
			_command   = command;
			_parameter = parameter;
		}

		public void Execute(None parameter)
		{
			_command(_parameter());
		}
	}
}