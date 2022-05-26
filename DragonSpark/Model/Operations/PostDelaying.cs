using DragonSpark.Compose;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class PostDelaying<TIn, TOut> : ISelecting<TIn, TOut>
{
	readonly ISelecting<TIn, TOut> _previous;
	readonly TimeSpan              _wait;

	public PostDelaying(ISelecting<TIn, TOut> previous) : this(previous, TimeSpan.FromSeconds(1)) {}

	public PostDelaying(ISelecting<TIn, TOut> previous, TimeSpan wait)
	{
		_previous = previous;
		_wait     = wait;
	}

	public async ValueTask<TOut> Get(TIn parameter)
	{
		var result = await _previous.Await(parameter);
		await Task.Delay(_wait).ConfigureAwait(false);
		return result;
	}
}