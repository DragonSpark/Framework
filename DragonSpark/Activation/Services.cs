using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;
using System;

namespace DragonSpark.Activation
{
	/*public interface IServiceLocator : Microsoft.Practices.ServiceLocation.IServiceLocator, IServiceRegistry, IObjectBuilder
	{}*/

	public static class Services
	{
		/*public static event EventHandler Assigned = delegate { };

		public static void Assign( IServiceLocator locator )
		{
			ServiceLocator.SetLocatorProvider( locator.Transform( x => new ServiceLocatorProvider( () => locator ) ) );
			Assigned( locator, EventArgs.Empty );
			Assigned = delegate { };
		}*/

		/*public static IServiceLocator Assign( IServiceLocator locator )
		{
			
			Instance = locator;
			return null;
		}

		static IServiceLocator Instance { get; set; }*/

		public static void Register<TFrom, TTo>()
		{
			With<IServiceLocator>( x => x.Register<TFrom, TTo>() );
		}

		public static void Register( Type from, Type to )
		{
			With<IServiceLocator>( x => x.Register( from, to ) );
		}

		public static void Register( Type type, object instance )
		{
			With<IServiceLocator>( x => x.Register( type, instance ) );
		}

		public static void Register<TService>( TService instance )
		{
			With<IServiceLocator>( x => x.Register( instance ) );
		}

		public static void Register<TService>( Func<TService> factory  )
		{
			With<IServiceLocator>( x => x.RegisterFactory( typeof(TService), () => factory() ) );
		}

		public static void With<TService>( Action<TService> action )
		{
			IsAvailable().IsTrue( () => Locate<TService>().NotNull( action ) );
		}

		public static TResult With<TService,TResult>( Func<TService,TResult> action )
		{
			var result = IsAvailable() ? Locate<TService>().Transform( action ) : default(TResult);
			return result;
		}

		public static TService Locate<TService>( string name = null )
		{
			var result = IsAvailable() ? ServiceLocator.Current.TryGetInstance<TService>( name ) : default( TService );
			return result;
		}

		public static bool IsAvailable()
		{
			return ServiceLocator.IsLocationProviderSet;
		}
	}
}