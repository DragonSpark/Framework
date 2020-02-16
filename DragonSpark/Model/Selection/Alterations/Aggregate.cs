using DragonSpark.Model.Sequences;

namespace DragonSpark.Model.Selection.Alterations
{
	public class Aggregate<TElement, T> : IAlteration<T> where TElement : ISelect<T, T>
	{
		readonly Array<TElement> _items;

		public Aggregate(Array<TElement> items) => _items = items;

		public T Get(T parameter)
		{
			var count  = _items.Length;
			var result = parameter;
			for (var i = 0; i < count; i++)
			{
				result = _items[i].Get(result);
			}

			return result;
		}
	}

	public class Aggregate<T> : Aggregate<ISelect<ISelect<T, T>, ISelect<T, T>>, ISelect<T, T>>
	{
		public Aggregate(Array<ISelect<ISelect<T, T>, ISelect<T, T>>> items) : base(items) {}
	}
}