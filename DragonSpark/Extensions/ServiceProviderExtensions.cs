using DragonSpark.Activation;
using DragonSpark.Activation.Location;
using DragonSpark.Sources.Parameterized.Caching;
using System;

namespace DragonSpark.Extensions
{
	public static class ServiceProviderExtensions
	{
		public static IActivator Cached( this IActivator @this ) => new DecoratedActivator( @this, new Cache<Type, object>( @this.GetService ).GetAssigned );

		public static T Get<T>( this IServiceProvider serviceProvider ) => Get<T>( serviceProvider, typeof(T) );

		public static T Get<T>( this IServiceProvider serviceProvider, Type type ) => (T)serviceProvider.GetService( type );
	}
}