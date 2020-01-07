using DragonSpark.Model;
using DragonSpark.Model.Commands;

namespace DragonSpark.Compose.Model
{
	sealed class Any : ICommand<object>
	{
		readonly ICommand<None> _command;

		public Any(ICommand<None> command) => _command = command;

		public void Execute(object _)
		{
			_command.Execute();
		}
	}
}