namespace DragonSpark.Model.Sequences.Query
{
	public interface IMaterialize<T> : IMaterialize<T, T> {}

	public interface IMaterialize<in TIn, out TOut> : IReduce<TIn, TOut[]> {}
}