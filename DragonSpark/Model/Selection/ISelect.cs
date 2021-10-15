using System;

namespace DragonSpark.Model.Selection;

public interface ISelect<in TIn, out TOut>
{
	TOut Get(TIn parameter);
}

public interface ISelect<in T, in TIn, out TOut> : ISelect<T, Func<TIn, TOut>?> {}