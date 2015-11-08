namespace DragonSpark.Activation
{
	public static class FactoryExtensions
	{
		public static T Create<T>( this IFactory @this, object parameter = null )
		{
			var result = (T)@this.Create( typeof(T), parameter );
			return result;
		}
	}
}