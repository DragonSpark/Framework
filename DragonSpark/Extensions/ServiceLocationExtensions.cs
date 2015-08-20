using DragonSpark.Activation;
using DragonSpark.Diagnostics;
using DragonSpark.Properties;
using Microsoft.Practices.ServiceLocation;
using System;

namespace DragonSpark.Extensions
{
	public static class ServiceLocationExtensions
	{
		public static T Assigned<T>( this T @this, bool force = false ) where T : IServiceLocator
		{
			var apply = !ServiceLocator.IsLocationProviderSet || force;
			apply.IsTrue( () => ServiceLocator.SetLocatorProvider( () => @this ) );
			return @this;
		}

		public static void Register<TFrom, TTo>( this IServiceLocator target, string name = null )
		{
			Register( target, typeof(TFrom), typeof(TTo), name );
		}

		public static void Register( this IServiceLocator target, Type from, Type to, string name = null )
		{
			target.TryGetInstance<IServiceRegistry>().NotNull( x => x.Register( from, to, name ) );
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
			catch ( ActivationException e )
			{
				// Log.Warning( string.Format( Resources.Activator_CouldNotActivate, type, name ?? Resources.Activator_None, e.GetMessage() ) );
			}
			return null;
		}
	}
}
