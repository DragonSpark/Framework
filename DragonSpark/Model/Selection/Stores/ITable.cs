namespace DragonSpark.Model.Selection.Stores;

public interface ITable<TIn, TOut> : IAssignable<TIn, TOut>
{
	bool Remove(TIn key);
}