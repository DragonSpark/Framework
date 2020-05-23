using DragonSpark.Compose;
using System;

namespace DragonSpark.Model.Commands
{
	public class Command<T> : ICommand<T>
	{
		readonly Action<T> _command;

		public Command(ICommand<T> command) : this(command.Execute) {}

		public Command(Action<T> command) => _command = command;

		public void Execute(T parameter)
		{
			_command(parameter);
		}
	}

	public class Command : ICommand
	{
		readonly Action _command;

		public Command(ICommand command) : this(command.Execute) {}

		public Command(Action command) => _command = command;

		public void Execute(None _)
		{
			_command();
		}
	}
}