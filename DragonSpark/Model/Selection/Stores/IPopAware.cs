namespace DragonSpark.Model.Selection.Stores;

public interface IPopAware<in TIn, TOut>
{
	bool TryPop(TIn parameter, out TOut result);
}