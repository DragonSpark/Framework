using DragonSpark.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Activation
{
	public static class Activator
	{
		public static IActivator Current => Services.Location.Locate<IActivator>() ?? SystemActivator.Instance;

		public static TResult Activate<TResult>( this IActivator @this )
		{
			return @this.Activate<TResult>( typeof(TResult) );
		}

		public static TResult Activate<TResult>( this IActivator @this, Type type )
		{
			var result = @this.Activate<TResult>( type, null );
			return result;
		}

		public static TResult Activate<TResult>( this IActivator @this, Type type, string name )
		{
			var result = @this.CanActivate( type, name ) ? (TResult)@this.Activate( type, name ) : default(TResult);
			return result;
		}

		public static TResult Construct<TResult>( this IActivator @this, params object[] parameters )
		{
			return Construct<TResult>( @this, typeof(TResult), parameters );
		}

		public static TResult Construct<TResult>( this IActivator @this, Type type, params object[] parameters )
		{
			var result = @this.CanConstruct( type, parameters ) ? (TResult)@this.Construct( type, parameters ) : default(TResult);
			return result;
		}

		public static IEnumerable<T> ActivateMany<T>( this IActivator @this, IEnumerable<Type> types )
		{
			var result = @this.ActivateMany( typeof(T), types ).Cast<T>();
			return result;
		}

		public static IEnumerable<object> ActivateMany( this IActivator @this, Type objectType, IEnumerable<Type> types )
		{
			var enumerable = types.Where( @objectType.Adapt().IsAssignableFrom );
			var result = enumerable.Select( @this.Activate<object> ).NotNull();
			return result;
		}
	} 
}