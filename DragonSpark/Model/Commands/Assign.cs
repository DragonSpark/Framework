using System;

namespace DragonSpark.Model.Commands;

public class Assign<TKey, TValue> : IAssign<TKey, TValue>
{
	readonly Action<TKey, TValue> _previous;

	public Assign(Action<TKey, TValue> previous) => _previous = previous;

	public void Execute(Pair<TKey, TValue> parameter)
	{
		_previous(parameter.Key, parameter.Value);
	}
}