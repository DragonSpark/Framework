using DragonSpark.Sources.Scopes;
using System;
using System.Reflection;

namespace DragonSpark.Sources
{
	public static class Factory
	{
		public static T Self<T>( this T @this ) => @this;
		/*public static TResult Self<TParameter, TResult>( this TResult @this, TParameter _ ) => @this;
		public static Func<TParameter, TResult> ToSelfDelegate<TParameter, TResult>( this TResult @this ) where TResult : class => @this.Self<TParameter, TResult>;*/

		public static Func<T> For<T>( T @this ) => ( typeof(T).GetTypeInfo().IsValueType ? new Source<T>( @this ) : @this.Sourced() ).Get;
	}
}