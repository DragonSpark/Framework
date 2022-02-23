using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Sequences;

public class AnyAwareSelect<TIn, TOut> : ISelecting<TIn, TOut>
{
	readonly ISelecting<TIn, TOut> _previous;
	readonly Await<TIn, bool>      _any;
	readonly TOut                  _default;

	protected AnyAwareSelect(Await<TIn, bool> any, ISelecting<TIn, TOut> previous, TOut @default)
	{
		_previous = previous;
		_any      = any;
		_default  = @default;
	}

	public async ValueTask<TOut> Get(TIn parameter)
	{
		var any    = await _any(parameter);
		var result = any ? await _previous.Await(parameter) : _default;
		return result;
	}
}