using DragonSpark.Model.Sequences.Query.Construction;

namespace DragonSpark.Model.Sequences.Query
{
	public static class Extensions
	{
		public static IContents<TIn, TOut> Returned<TIn, TOut>(this IContents<TIn, TOut> @this)
			=> new ReturnedContents<TIn, TOut>(@this);

		public static IContent<TIn, TOut> Returned<TIn, TOut>(this IContent<TIn, TOut> @this)
			=> new ReturnedContent<TIn, TOut>(@this);
	}
}