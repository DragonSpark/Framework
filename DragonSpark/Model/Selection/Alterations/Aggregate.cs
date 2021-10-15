using DragonSpark.Compose;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Model.Selection.Alterations;

public class Aggregate<TElement, T> : IAlteration<T> where TElement : ISelect<T, T>
{
	readonly uint            _length;
	readonly Array<TElement> _items;

	public Aggregate(params TElement[] items) : this(items.Length.Grade(), items) {}

	public Aggregate(uint length, params TElement[] items)
	{
		_length = length;
		_items  = items;
	}

	public T Get(T parameter)
	{
		var result = parameter;

		for (var i = 0u; i < _length; i++)
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