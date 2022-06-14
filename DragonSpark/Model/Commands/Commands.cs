namespace DragonSpark.Model.Commands;

public class Commands<T> : ICommand<T>
{
	readonly ICommand<T>[] _items;
	readonly uint        _length;

	public Commands(params ICommand<T>[] items) : this(items, (uint)items.Length) {}

	public Commands(ICommand<T>[] items, uint length)
	{
		_items  = items;
		_length = length;
	}

	public void Execute(T parameter)
	{
		for (var i = 0; i < _length; i++)
		{
			_items[i].Execute(parameter);
		}
	}
}