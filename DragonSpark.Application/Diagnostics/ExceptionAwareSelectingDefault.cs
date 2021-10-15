using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Diagnostics;

public class ExceptionAwareSelectingDefault<TIn, TOut> : ISelecting<TIn, TOut>
{
	readonly ExceptionAwareSelecting<TIn, TOut> _previous;
	readonly TOut                               _default;

	public ExceptionAwareSelectingDefault(ExceptionAwareSelecting<TIn, TOut> previous, TOut @default)
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
		catch (Exception)
		{
			return _default;
		}
	}
}