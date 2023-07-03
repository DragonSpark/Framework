namespace DragonSpark.Model.Results;

public sealed class ProtectedAwareMutable<T> : IMutable<T>
{
	readonly IMutable<T> _previous;

	public ProtectedAwareMutable() : this(new Variable<T>()!) {}

	public ProtectedAwareMutable(IMutable<T> previous) => _previous = previous;

	public T Get()
	{
		lock (_previous)
		{
			return _previous.Get();
		}
	}

	public void Execute(T parameter)
	{
		lock (_previous)
		{
			_previous.Execute(parameter);
		}
	}
}