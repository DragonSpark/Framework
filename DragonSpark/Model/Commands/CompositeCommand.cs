using DragonSpark.Model.Sequences;

namespace DragonSpark.Model.Commands
{
	public class CompositeCommand<T> : ICommand<T>
	{
		readonly Array<ICommand<T>> _items;
		readonly uint               _length;

		public CompositeCommand(params ICommand<T>[] items) : this(new Array<ICommand<T>>(items)) {}

		public CompositeCommand(Array<ICommand<T>> items) : this(items, items.Length) {}

		public CompositeCommand(Array<ICommand<T>> items, uint length)
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
}