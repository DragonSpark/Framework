using DragonSpark.Extensions;
using System;
using System.Composition;
using System.Composition.Hosting.Core;

namespace DragonSpark.Composition
{
	public static class CompositionHostExtensions
	{
		public static T TryGet<T>( this CompositionContext @this, string name = null ) => TryGet<T>( @this, typeof(T), name );

		public static T TryGet<T>( this CompositionContext @this, Type type, string name = null )
		{
			object existing;
			var result = @this.TryGetExport( type, name, out existing ) ? (T)existing : default(T);
			return result;
		}

		public static object Registered( this LifetimeContext @this, object instance )
		{
			instance.As<IDisposable>( @this.AddBoundInstance );
			return instance;
		}
	}
}