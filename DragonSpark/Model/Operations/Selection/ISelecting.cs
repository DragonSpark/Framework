using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Operations.Selection;

public interface ISelecting<in TIn, TOut> : ISelect<TIn, ValueTask<TOut>>;