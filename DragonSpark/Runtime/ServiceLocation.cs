using System;
using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Runtime
{
	public static class ServiceLocation
	{
		public static event EventHandler Assigned = delegate { };

		public static void Assign( IServiceLocator locator )
		{
			ServiceLocator.SetLocatorProvider( () => locator );
			Assigned( locator, EventArgs.Empty );
			Assigned = delegate { };
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
			try
			{
				return ServiceLocator.Current != null;
			}
			catch ( NullReferenceException )
			{
				return false;
			}
		}
	}
}