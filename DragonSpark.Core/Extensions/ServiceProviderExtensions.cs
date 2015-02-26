using System;

namespace DragonSpark.Extensions
{
	public static class ServiceProviderExtensions
	{
		public static TService Get<TService>( this IServiceProvider serviceProvider )
		{
			var result = serviceProvider.GetService( typeof(TService) ).To<TService>();
			return result;
		}

		/*public static TService Get<TService>( this object @this )
		{
			var result = @this.AsTo<IServiceProvider, TService>( x => x.Get<TService>() );
			return result;
		}*/
	}
}