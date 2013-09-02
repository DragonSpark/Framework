using DragonSpark.IoC;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Linq;

namespace DragonSpark.Extensions
{
	public static class ServiceLocationExtensions
	{
		public static TServiceLocator Initialized<TServiceLocator>( this TServiceLocator target ) where TServiceLocator : IServiceLocator
		{
			target.GetInstance<object>();
			return target;
		}

		public static IServiceLocator AsCurrent( this IServiceLocator target )
		{
			ServiceLocator.SetLocatorProvider( () => target );
			return target;
		}

		public static TItem GetInstanceWithInheritance<TItem>( this IServiceLocator target ) where TItem : class
		{
			var types = typeof(TItem).ResolveInterfaces();
			var result = types.Select( x => target.TryGetInstance(x) ).NotNull().FirstOrDefaultOfType<TItem>();
			return result;
		}

		public static void Register<TFrom, TTo>( this IServiceLocator target )
		{
			target.TryGetInstance<IServiceRegistry>().NotNull( x => x.Register( typeof(TFrom), typeof(TTo) ) );
		}

		public static void Register( this IServiceLocator target, Type type, object instance )
		{
			target.TryGetInstance<IServiceRegistry>().NotNull( x => x.Register( type, instance ) );
		}

		public static void RegisterFactory( this IServiceLocator target, Type type, Func<object> factory )
		{
			target.TryGetInstance<IServiceRegistry>().NotNull( x => x.RegisterFactory( type, factory ) );
		}

		public static void Register<TService>( this IServiceLocator target, TService instance )
		{
			target.TryGetInstance<IServiceRegistry>().NotNull( x => x.Register( typeof(TService), instance ) );
		}

		public static TItem TryGetInstance<TItem>( this IServiceLocator target, string name = null )
		{
			var result = target.TryGetInstance( typeof(TItem), name ).To<TItem>();
			return result;
		}

		public static object TryGetInstance( this IServiceLocator target, Type type, string name = null )
		{
			try
			{
				var result = target.GetInstance( type, name );
				return result;
			}
			catch ( NullReferenceException ) {}
			catch ( ActivationException ) {}
			return null;
		}
	}
}
