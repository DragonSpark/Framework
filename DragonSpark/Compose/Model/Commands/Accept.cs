using DragonSpark.Model;
using DragonSpark.Model.Commands;

namespace DragonSpark.Compose.Model.Commands;

sealed class Accept<T> : ICommand<T>
{
	readonly ICommand<None> _command;

	public Accept(ICommand<None> command) => _command = command;

	public void Execute(T _)
	{
		_command.Execute();
	}
}