using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Selection;

public class SelectingToken<T, TOut> : Selecting<Token<T>, Token<TOut>>, ISelectingToken<T, TOut>
{
	public SelectingToken(ISelect<Token<T>, ValueTask<Token<TOut>>> select) : base(select) {}

	public SelectingToken(Func<Token<T>, ValueTask<Token<TOut>>> select) : base(select) {}
}