using DragonSpark.Model;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Runtime.Execution;

public class FirstAssigned : ICondition
{
	readonly IMutable<bool> _store;
	readonly bool           _target;

	protected FirstAssigned(IMutable<bool> store) : this(store, store.Get()) {}

	protected FirstAssigned(IMutable<bool> store, bool target)
	{
		_store  = store;
		_target = target;
	}

	public bool Get(None parameter)
	{
		var result = _store.Get() == _target;
		if (result)
		{
			_store.Execute(!_target);
		}

		return result;
	}
}