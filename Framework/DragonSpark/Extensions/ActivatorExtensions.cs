using DragonSpark.Activation;

namespace DragonSpark.Extensions
{
	public static class ActivatorExtensions
	{
		public static bool CanActivate<T>( this IActivator @this, string name = null )
		{
			var result = @this.CanActivate( typeof(T), name );
			return result;
		}
	}
}