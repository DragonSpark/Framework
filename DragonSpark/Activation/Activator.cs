using DragonSpark.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Activation
{
	public static class Activator
	{
		public static IActivator GetCurrent() => Services.Location.Locate<IActivator>() ?? SystemActivator.Instance; // TODO: Figure out elegant way to remove this.

		public static TResult Activate<TResult>() => Activate<TResult>( GetCurrent() );

		public static TResult Activate<TResult>( this IActivator @this ) => @this.Activate<TResult>( typeof(TResult) );

		public static TResult Activate<TResult>( this IActivator @this, Type type ) => @this.Activate<TResult>( type, null );

		public static TResult Activate<TResult>( this IActivator @this, Type type, string name ) => @this.CanActivate( type, name ) ? (TResult)@this.Activate( type, name ) : default( TResult );

		public static TResult Construct<TResult>( this IActivator @this, params object[] parameters ) => Construct<TResult>( @this, typeof(TResult), parameters );

		public static TResult Construct<TResult>( this IActivator @this, Type type, params object[] parameters ) => @this.CanConstruct( type, parameters ) ? (TResult)@this.Construct( type, parameters ) : default( TResult );

		public static IEnumerable<T> ActivateMany<T>( this IActivator @this, IEnumerable<Type> types ) => @this.ActivateMany( typeof( T ), types ).Cast<T>();

		public static IEnumerable<object> ActivateMany( this IActivator @this, Type objectType, IEnumerable<Type> types )
		{
			var enumerable = types.Where( @objectType.Adapt().IsAssignableFrom );
			var result = enumerable.Select( @this.Activate<object> ).NotNull();
			return result;
		}
	} 
}