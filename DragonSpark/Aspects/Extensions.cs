using DragonSpark.Model.Selection;

namespace DragonSpark.Aspects
{
	public static class Extensions
	{
		public static ISelect<TIn, TOut> Configured<TIn, TOut>(this ISelect<TIn, TOut> @this)
			=> Aspects<TIn, TOut>.Default.Get(@this).Get(@this);

		public static IAspect<TIn, TOut> Registered<TIn, TOut>(this IAspect<TIn, TOut> @this)
		{
			AspectRegistry.Default.Execute(new Registration<TIn, TOut>(@this));
			return @this;
		}
	}
}