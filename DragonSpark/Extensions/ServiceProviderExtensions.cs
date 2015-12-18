using System;

namespace DragonSpark.Extensions
{
	public static class ServiceProviderExtensions
	{
		public static TService Get<TService>( this IServiceProvider serviceProvider ) where TService : class
		{
			var result = serviceProvider.GetService( typeof(TService) ).As<TService>();
			return result;
		}

		/*public static TService Get<TService>( this object @this )
		{
			var result = @this.AsTo<IServiceProvider, TService>( x => x.Get<TService>() );
			return result;
		}*/
	}
}