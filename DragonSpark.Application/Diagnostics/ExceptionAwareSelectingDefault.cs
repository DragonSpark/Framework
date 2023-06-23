using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Application.Diagnostics;

public sealed class ExceptionAwareSelectingDefault<TIn, TOut> : ISelecting<TIn, TOut>
{
	readonly ISelecting<TIn, TOut> _previous;
	readonly TOut                  _default;

	public ExceptionAwareSelectingDefault(ISelecting<TIn, TOut> previous, TOut @default)
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