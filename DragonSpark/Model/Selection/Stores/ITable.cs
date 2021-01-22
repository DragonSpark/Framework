using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Model.Selection.Stores
{
	public interface ITable<TIn, TOut> : IConditional<TIn, TOut>, IAssign<TIn, TOut>
	{
		bool Remove(TIn key);
	}

	public class TableVariable<TIn, TOut> : IMutable<TOut?>
	{
		readonly TIn                _key;
		readonly ITable<TIn, TOut?> _store;

		public TableVariable(TIn key, ITable<TIn, TOut?> store)
		{
			_key   = key;
			_store = store;
		}

		public TOut? Get() => _store.TryGet(_key, out var result) ? result : default;

		public void Execute(TOut? parameter)
		{
			_store.Assign(_key, parameter);
		}
	}
}