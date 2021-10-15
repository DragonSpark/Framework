using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Diagnostics;

public class ExceptionSelectingDefault<TIn, TOut> : ISelecting<TIn, TOut>
{
	readonly ISelecting<TIn, TOut> _previous;
	readonly TOut                  _default;

	protected ExceptionSelectingDefault(ExceptionAwareSelecting<TIn, TOut> previous, TOut @default)
	{
		_previous = previous;
		_default  = @default;
	}

	public async ValueTask<TOut> Get(TIn parameter)
	{
		try
		{
			return await _previous.Await(parameter);
		}
		// ReSharper disable once CatchAllClause
		catch
		{
			return _default;
		}
	}
}