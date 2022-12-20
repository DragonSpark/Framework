using DragonSpark.Reflection.Types;

namespace DragonSpark.Testing.Objects.Entities.Generation.Compose;

public sealed class Scope<T, TOther> where TOther : class
{
	readonly IRule<T, TOther>    _rule;
	readonly ITypedTable<object> _store;

	public Scope(IRule<T, TOther> rule, ITypedTable<object> store)
	{
		_rule  = rule;
		_store = store;
	}

	public IRule<T, TOther> PerCall() => _rule;

	public IRule<T, TOther> Once() => new StoredRule<T, TOther>(_rule, _store);
}