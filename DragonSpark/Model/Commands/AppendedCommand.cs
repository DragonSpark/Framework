namespace DragonSpark.Model.Commands;

public class AppendedCommand<T> : ICommand<T>
{
	readonly ICommand<T> _previous;
	readonly ICommand<T> _next;

	public AppendedCommand(ICommand<T> previous, ICommand<T> next)
	{
		_previous = previous;
		_next     = next;
	}

	public void Execute(T parameter)
	{
		_previous.Execute(parameter);
		_next.Execute(parameter);
	}
}