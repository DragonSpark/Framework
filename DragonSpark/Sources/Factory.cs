using DragonSpark.Sources.Scopes;
using System;
using System.Reflection;

namespace DragonSpark.Sources
{
	public static class Factory
	{
		public static T Self<T>( this T @this ) => @this;
		public static TResult Shift<TParameter, TResult>( this TResult @this, TParameter _ ) => @this;

		public static Func<T> For<T>( T @this ) => ( typeof(T).GetTypeInfo().IsValueType ? new Source<T>( @this ) : @this.Sourced() ).Get;
	}
}