using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Selection;

public class AssumingSelector<TIn, TOut> : ISelecting<TIn, TOut>
{
	readonly IResulting<ISelect<TIn, TOut>> _previous;

	protected AssumingSelector(IResulting<ISelect<TIn, TOut>> previous) => _previous = previous;

	public async ValueTask<TOut> Get(TIn parameter)
	{
		var previous = await _previous.Await();
		return previous.Get(parameter);
	}
}