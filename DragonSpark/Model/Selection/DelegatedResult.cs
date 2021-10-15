using System;

namespace DragonSpark.Model.Selection;

public class DelegatedResult<TIn, TOut> : ISelect<TIn, TOut>
{
	readonly Func<TOut> _result;

	public DelegatedResult(Func<TOut> source) => _result = source;

	public TOut Get(TIn _) => _result();
}