using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Reflection.Types;
using System.Reflection;

namespace DragonSpark.Application.Entities.Generation;

sealed class StateAwareGenerate<T, TOther> : ISelect<T, TOther> where T : class where TOther : class
{
	readonly ISelect<T, TOther>       _previous;
	readonly ITable<TypeInfo, object> _state;
	readonly TypeInfo                 _key;

	public StateAwareGenerate(ISelect<T, TOther> previous, ITypedTable<object> state)
		: this(previous, state, A.Metadata<TOther>()) {}

	public StateAwareGenerate(ISelect<T, TOther> previous, ITable<TypeInfo, object> state, TypeInfo key)
	{
		_previous = previous;
		_state    = state;
		_key      = key;
	}

	public TOther Get(T parameter)
	{
		var key = A.Metadata<T>();
		_state.Assign(key, parameter);
		var result = _state.IsSatisfiedBy(_key) ? _state.Get(_key).To<TOther>() : _previous.Get(parameter);
		_state.Remove(key);
		return result;
	}
}