using System;
using DragonSpark.Runtime;

namespace DragonSpark.Model.Commands
{
	sealed class FixedParameterCommand<T> : ICommand
	{
		readonly Action<T> _command;
		readonly T         _parameter;

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
}