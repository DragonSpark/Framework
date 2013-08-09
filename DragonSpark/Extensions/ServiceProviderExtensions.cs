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
	}
}