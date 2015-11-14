using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;
using System;

namespace DragonSpark.Activation
{
	public static class Services
	{
		public static void Initialize( IServiceLocationHost host )
		{
			Host = host;
		}	static IServiceLocationHost Host { get; set; } = ServiceLocationHost.Instance;

		public static IServiceLocation Location => Host.Location;
	}

	public static class ServiceLocatorHostExtensions
	{
		public static bool IsAvailable( this IServiceLocation @this )
		{
			var result = @this.Transform( x => x.IsAvailable );
			return result;
		}

		public static void With<TService>( this IServiceLocation @this, Action<TService> action )
		{
			@this.IsAvailable().IsTrue( () => @this.Locate<TService>().With( action ) );
		}

		public static TResult With<TService, TResult>( this IServiceLocation @this, Func<TService, TResult> action )
		{
			var result = @this.IsAvailable() ? @this.Locate<TService>().Transform( action ) : default(TResult);
			return result;
		}

		public static TService Locate<TService>( this IServiceLocation @this, string name = null )
		{
			var result = @this.IsAvailable() ? @this.Locator.TryGetInstance<TService>( name ) : default(TService);
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

		void Assign( IServiceLocator instance );
	}

	public class ServiceLocation : IServiceLocation
	{
		public static ServiceLocation Instance { get; } = new ServiceLocation();

		public bool IsAvailable => ServiceLocator.IsLocationProviderSet;

	    public IServiceLocator Locator => ServiceLocator.Current;

		public void Assign( IServiceLocator instance )
		{
			ServiceLocator.SetLocatorProvider( instance != null ? () => instance : (ServiceLocatorProvider)null );
		}
	}

	public interface IServiceLocationHost
	{
		IServiceLocation Location { get; }
	}

	public class ServiceLocationHost : IServiceLocationHost
	{
		public static ServiceLocationHost Instance { get; } = new ServiceLocationHost();

		public IServiceLocation Location => ServiceLocation.Instance;
	}


}