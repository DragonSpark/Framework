using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Selection;

public class Continuing<T, TOut> : Selecting<Token<T>, Token<TOut>>, IContinuing<T, TOut>
{
	public Continuing(ISelect<Token<T>, ValueTask<Token<TOut>>> select) : base(select) {}

	public Continuing(Func<Token<T>, ValueTask<Token<TOut>>> select) : base(select) {}
}