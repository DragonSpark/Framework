using DragonSpark.Model.Sequences;

namespace DragonSpark.Model.Selection.Stores;

public interface ITableValues<TIn, TOut> : ITable<TIn, TOut>
{
	IArray<TOut> Values { get; }
}