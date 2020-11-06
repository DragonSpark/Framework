using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	public class Referencing<TIn, TOut> : ISelecting<TIn, TOut> where TIn : class where TOut : class
	{
		readonly ISelecting<TIn, TOut> _previous;
		readonly ITable<TIn, TOut>     _store;

		public Referencing(ISelecting<TIn, TOut> previous) : this(previous, new ReferenceValueTable<TIn, TOut>()) {}

		public Referencing(ISelecting<TIn, TOut> previous, ITable<TIn, TOut> store)
		{
			_previous = previous;
			_store    = store;
		}

		public async ValueTask<TOut> Get(TIn parameter)
			=> _store.TryPop(parameter, out var existing) ? existing : await Access(parameter);

		async ValueTask<TOut> Access(TIn parameter)
		{
			var result = await _previous.Get(parameter);
			_store.Assign(parameter, result);
			return result;
		}
	}
}