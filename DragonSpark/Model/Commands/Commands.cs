using DragonSpark.Model.Sequences;

namespace DragonSpark.Model.Commands;

public class Commands<T> : ICommand<T>
{
	readonly Array<ICommand<T>> _items;
	readonly uint               _length;

	public Commands(params ICommand<T>[] items) : this(new Array<ICommand<T>>(items)) {}

	public Commands(Array<ICommand<T>> items) : this(items, items.Length) {}

	public Commands(Array<ICommand<T>> items, uint length)
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