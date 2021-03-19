using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	sealed class Introduce<TIn, TOut> : ISelecting<TIn, (TIn, TOut)>
	{
		readonly Await<TIn, TOut> _select;

		public Introduce(ISelect<TIn, ValueTask<TOut>> select) : this(select.Await) {}

		public Introduce(Await<TIn, TOut> select) => _select = select;

		public async ValueTask<(TIn, TOut)> Get(TIn parameter) => (parameter, await _select(parameter));
	}

}
