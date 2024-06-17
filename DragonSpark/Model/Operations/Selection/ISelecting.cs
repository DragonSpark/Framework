using DragonSpark.Compose;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Selection;

public interface ISelecting<in TIn, TOut> : ISelect<TIn, ValueTask<TOut>>;

// TODO

public interface ISelectingToken<T, TOut> : ISelecting<Token<T>, Token<TOut>>;

public class SelectingToken<T, TOut> : Selecting<Token<T>, Token<TOut>>, ISelectingToken<T, TOut>
{
	public SelectingToken(ISelect<Token<T>, ValueTask<Token<TOut>>> select) : base(select) {}

	public SelectingToken(Func<Token<T>, ValueTask<Token<TOut>>> select) : base(select) {}
}

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