using DragonSpark.Compose;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Selection;

sealed class TokenAdapter<TIn, TOut> : ISelectingToken<TIn, TOut>
{
	readonly ISelect<TIn, ValueTask<TOut>> _previous;

	public TokenAdapter(ISelect<TIn, ValueTask<TOut>> previous) => _previous = previous;

	public async ValueTask<Token<TOut>> Get(Token<TIn> parameter)
	{
		var (subject, token) = parameter;
		return new(await _previous.Await(subject), token);
	}
}