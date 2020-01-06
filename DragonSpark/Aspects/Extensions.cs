namespace DragonSpark.Aspects
{
	public static class Extensions
	{
		public static IAspect<TIn, TOut> Registered<TIn, TOut>(this AspectRegistry @this, IAspect<TIn, TOut> aspect)
		{
			@this.Execute(new Registration<TIn, TOut>(aspect));
			return aspect;
		}
	}
}