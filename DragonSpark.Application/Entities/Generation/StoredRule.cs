using Bogus;
using DragonSpark.Compose;
using DragonSpark.Reflection.Types;
using System.Reflection;

namespace DragonSpark.Application.Entities.Generation;

sealed class StoredRule<T, TOther> : IRule<T, TOther> where TOther : class
{
	readonly IRule<T, TOther>    _previous;
	readonly ITypedTable<object> _store;
	readonly TypeInfo            _key;

	public StoredRule(IRule<T, TOther> previous, ITypedTable<object> store)
		: this(previous, store, A.Type<TOther>().GetTypeInfo()) {}

	public StoredRule(IRule<T, TOther> previous, ITypedTable<object> store, TypeInfo key)
	{
		_previous = previous;
		_store    = store;
		_key      = key;
	}

	public TOther Get((Faker, T) parameter)
	{
		if (!_store.TryGet(_key, out var existing))
		{
			var value = _previous.Get(parameter);
			_store.Assign(_key, value);
			return value;
		}

		var result = existing.To<TOther>();
		return result;
	}
}