using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Sequences.Query;

public interface IYield<in TIn, out TOut> : ISelect<TIn, IEnumerable<TOut>>;