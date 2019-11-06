using DragonSpark.Model.Commands;
using DragonSpark.Runtime;

namespace DragonSpark.Model.Selection.Adapters
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