using DragonSpark.Model.Sequences;

namespace DragonSpark.Model.Commands
{
	public class CompositeCommand<T> : ICommand<T>
	{
		readonly Array<ICommand<T>> _items;

		public CompositeCommand(params ICommand<T>[] items) : this(new Array<ICommand<T>>(items)) {}

		public CompositeCommand(Array<ICommand<T>> items) => _items = items;

		public void Execute(T parameter)
		{
			var length = _items.Length;
			for (var i = 0u; i < length; i++)
			{
				_items[i].Execute(parameter);
			}
		}
	}
}