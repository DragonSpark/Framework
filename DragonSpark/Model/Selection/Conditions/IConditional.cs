namespace DragonSpark.Model.Selection.Conditions
{
	public interface IConditional<in TIn, out TOut> : IConditionAware<TIn>, ISelect<TIn, TOut> {}
}