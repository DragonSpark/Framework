using DragonSpark.Application.Entities.Generation;
using DragonSpark.Reflection.Types;

namespace DragonSpark.Application.Compose.Entities.Generation
{
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
}