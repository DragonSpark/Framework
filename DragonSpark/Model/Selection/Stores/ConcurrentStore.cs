namespace DragonSpark.Model.Selection.Stores;

public class ConcurrentStore<TIn, TOut> : Select<TIn, TOut> where TIn : class where TOut : class?
{
	protected ConcurrentStore(ISelect<TIn, TOut> select) : this(select.Get) {}

	protected ConcurrentStore(Func<TIn, TOut> source) : base(new ConcurrentTable<TIn, TOut>(source)) {}
}