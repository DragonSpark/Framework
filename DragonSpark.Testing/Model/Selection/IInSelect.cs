namespace DragonSpark.Testing.Model.Selection;

public interface IInSelect<TIn, out TOut>
{
	TOut Get(in TIn parameter);
}