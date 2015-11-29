using DragonSpark.Extensions;
using DragonSpark.Runtime;
using Microsoft.Practices.ServiceLocation;
using System;

namespace DragonSpark.Activation
{
	public static class Services
	{
		public static IServiceLocation Location => AmbientValues.Get<IServiceLocation>() ?? ServiceLocation.Instance;
	}

	public static class ServiceLocatorExtensions
	{
		/*public static bool IsAvailable( this IServiceLocation @this )
		{
			var result = @this.Transform( x => x.IsAvailable );
			return result;
		}*/

		public static void With<TService>( this IServiceLocation @this, Action<TService> action )
		{
			@this.IsAvailable.IsTrue( () => @this.Locate<TService>().With( action ) );
		}

		public static TResult With<TService, TResult>( this IServiceLocation @this, Func<TService, TResult> action )
		{
			var result = @this.IsAvailable ? @this.Locate<TService>().Transform( action ) : default(TResult);
			return result;
		}

		public static TService Locate<TService>( this IServiceLocation @this, string name = null )
		{
			var result = @this.IsAvailable ? @this.Locator.GetInstance<TService>( name ) : default(TService);
			return result;
		}

		public static void Register<TFrom, TTo>( this IServiceLocation @this )
		{
			@this.With<IServiceLocator>( x => x.Register<TFrom, TTo>() );
		}

		public static void Register( this IServiceLocation @this, Type from, Type to )
		{
			@this.With<IServiceLocator>( x => x.Register( from, to ) );
		}

		public static void Register( this IServiceLocation @this, Type type, object instance )
		{
			@this.With<IServiceLocator>( x => x.Register( type, instance ) );
		}

		public static void Register<TService>( this IServiceLocation @this, TService instance )
		{
			@this.With<IServiceLocator>( x => x.Register( instance ) );
		}

		public static void Register<TService>( this IServiceLocation @this, Func<TService> factory )
		{
			@this.With<IServiceLocator>( x => x.RegisterFactory( typeof(TService), () => factory() ) );
		}
	}

	public interface IServiceLocation
	{
		bool IsAvailable { get; }

		IServiceLocator Locator { get; }

		void Assign( IServiceLocator locator );
	}

	public class ServiceLocation : IServiceLocation
	{
		public static ServiceLocation Instance { get; } = new ServiceLocation();

		ServiceLocation()
		{
			ServiceLocator.SetLocatorProvider( AmbientValues.Get<IServiceLocator> );
		}

		public void Assign( IServiceLocator locator )
		{
			if ( IsAvailable )
			{
				AmbientValues.Remove( Locator );
			}

			locator.With( context =>
			{
				var keyLocator = context.GetInstance<IAmbientKeyLocator>() ?? AmbientKeyLocator.Instance;
				var key = keyLocator.Locate( context ) ?? new AmbientKey<IServiceLocator>( new EqualitySpecification( context ) );
				AmbientValues.Register( key, context );
			} );
		}

		public bool IsAvailable => ServiceLocator.IsLocationProviderSet && Locator != null;

		public IServiceLocator Locator => ServiceLocator.Current;
	}

	public interface IAmbientKeyLocator
	{
		IAmbientKey Locate( IServiceLocator context );
	}

	class AmbientKeyLocator : IAmbientKeyLocator
	{
		public static AmbientKeyLocator Instance { get; } = new AmbientKeyLocator();

		public IAmbientKey Locate( IServiceLocator context )
		{
			var result = context.GetInstance<IAmbientKey>();
			return result;
		}
	}

	/*public interface IServiceLocationHost
	{
		IServiceLocation Location { get; }
	}

	public class ServiceLocationHost : IServiceLocationHost
	{
		public static ServiceLocationHost Instance { get; } = new ServiceLocationHost();

		public IServiceLocation Location => ServiceLocation.Instance;
	}*/


}