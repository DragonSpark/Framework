using System;
using System.Linq;
using Microsoft.Practices.ServiceLocation;

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
