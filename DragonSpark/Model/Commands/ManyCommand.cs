using System;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Model.Commands
{
	public sealed class ManyCommand<T> : ICommand<Store<T>>
	{
		readonly Action<T> _command;

		public ManyCommand(ICommand<T> command) : this(command.Execute) {}

		public ManyCommand(Action<T> command) => _command = command;

		public void Execute(Store<T> parameter)
		{
			var length   = parameter.Length;
			var instance = parameter.Instance;
			for (var i = 0u; i < length; i++)
			{
				_command(instance[i]);
			}
		}
	}
}