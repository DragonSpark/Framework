using DragonSpark.Activation;
using DragonSpark.Activation.Location;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized.Caching;
using System;

namespace DragonSpark.Extensions
{
	public static class ServiceProviderExtensions
	{
		public static IActivator Cached( this IActivator @this ) => new DecoratedActivator( @this, new Cache<Type, object>( @this.GetService ).GetAssigned );

		public static T Get<T>( this ISource<IServiceProvider> @this ) => @this.GetService( typeof(T) ).As<T>();
		public static object GetService( this ISource<IServiceProvider> @this, Type type ) => @this.Get().GetService( type ).Account( type );

		public static T Get<T>( this IServiceProvider @this ) => @this.GetService( typeof(T) ).As<T>();

		public static T Get<T>( this IServiceProvider @this, Type type ) => @this.GetService( type ).Account<T>();
	}
}