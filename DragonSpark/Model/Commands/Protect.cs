namespace DragonSpark.Model.Commands;

public sealed class Protect<T> : ICommand<T>
{
	readonly ICommand<T> _previous;
	readonly object      _lock;

	public Protect(ICommand<T> previous) : this(previous, new()) {}

	public Protect(ICommand<T> previous, object @lock)
	{
		_previous  = previous;
		_lock = @lock;
	}

	public void Execute(T parameter)
	{
		lock (_lock)
		{
			_previous.Execute(parameter);
		}
	}
}