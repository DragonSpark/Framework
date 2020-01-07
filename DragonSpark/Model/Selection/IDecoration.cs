namespace DragonSpark.Model.Selection
{
	public interface IDecoration<TIn, TOut> : ISelect<(TIn, TOut), TOut> {}
}