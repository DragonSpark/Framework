namespace DragonSpark
{
	public static class PriorityExtensions
	{
		public static T WithPriority<T>( this T @this, Priority priority ) => WithPriority( @this, new DefaultPriorityAware( priority ) );

		public static T WithPriority<T>( this T @this, IPriorityAware aware )
		{
			AssociatedPriority.Default.Set( @this, aware );
			return @this;
		}
	}
}