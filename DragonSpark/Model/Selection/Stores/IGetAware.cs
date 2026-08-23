namespace DragonSpark.Model.Selection.Stores;

public interface IGetAware<in TIn, TOut>
{
	bool TryGet(TIn parameter, out TOut result);
}