using DragonSpark.Activation;
using Microsoft.Practices.ServiceLocation;
using System;

namespace DragonSpark.Extensions
{
	public static class ServiceLocationExtensions
	{
		public static void Register<TFrom, TTo>( this IServiceLocator target, string name = null ) => Register( target, typeof(TFrom), typeof(TTo), name );

		public static void Register( this IServiceLocator target, Type from, Type to, string name = null ) => With( target, x => x.Register( @from, to, name ) );

		public static void Register( this IServiceLocator target, Type type, object instance ) => With( target, x => x.Register( type, instance ) );

		public static void Register<TService>( this IServiceLocator target, TService instance ) => target.Register( typeof(TService), instance );

		public static void RegisterFactory( this IServiceLocator target, Type type, Func<object> factory ) => With( target, x => x.RegisterFactory( type, factory ) );

		static void With( IServiceLocator locator, Action<IServiceRegistry> action ) => locator.GetInstance<IServiceRegistry>().With( action );
	}
}
