using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Sequences.Query
{
	public interface IReduce<T> : IReduce<T, T> {}

	public interface IReduce<in TIn, out TOut> : ISelect<TIn[], TOut> {}
}