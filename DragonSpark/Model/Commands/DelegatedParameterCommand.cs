using System;
using DragonSpark.Runtime;

namespace DragonSpark.Model.Commands
{
	public class DelegatedParameterCommand<T> : ICommand
	{
		readonly Action<T> _command;
		readonly Func<T>   _parameter;

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