using DragonSpark.Activation;
using System;

namespace DragonSpark.Extensions
{
	public static class ServiceLocatorExtensions
	{
		public static TService Locate<TService>( this IServiceLocation @this ) => Locate<TService>( @this, null );

		public static TService Locate<TService>( this IServiceLocation @this, string name ) => @this.IsAvailable ? @this.Item.GetInstance<TService>( name ) : default(TService);
	}
}