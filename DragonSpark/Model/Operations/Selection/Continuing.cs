using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Selection;

public class Continuing<T, TOut> : Selecting<Stop<T>, Stop<TOut>>, IContinuing<T, TOut>
{
	public Continuing(ISelect<Stop<T>, ValueTask<Stop<TOut>>> select) : base(select) {}

	public Continuing(Func<Stop<T>, ValueTask<Stop<TOut>>> select) : base(select) {}
}