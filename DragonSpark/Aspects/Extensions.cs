namespace DragonSpark.Aspects
{
	public static class Extensions
	{
		/*public static ISelect<TIn, TOut> Configured<TIn, TOut>(this ISelect<TIn, TOut> @this)
			=> Aspects<TIn, TOut>.Default.Get(@this).Get(@this);*/

		public static IAspect<TIn, TOut> Registered<TIn, TOut>(this AspectRegistry @this, IAspect<TIn, TOut> aspect)
		{
			@this.Execute(new Registration<TIn, TOut>(aspect));
			return aspect;
		}
	}
}