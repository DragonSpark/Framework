namespace DragonSpark.Model.Sequences.Query.Construction
{
	public sealed class ReturnedContents<TIn, TOut> : IContents<TIn, TOut>
	{
		readonly IContents<TIn, TOut> _contents;

		public ReturnedContents(IContents<TIn, TOut> projections) => _contents = projections;

		public IContent<TIn, TOut> Get(Parameter<TIn, TOut> parameter) => _contents.Get(parameter).Returned();
	}
}