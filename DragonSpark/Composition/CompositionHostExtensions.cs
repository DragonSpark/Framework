using System;
using System.Composition;
using System.Composition.Hosting;
using System.Composition.Hosting.Core;
using DragonSpark.Extensions;

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

		public static ContainerConfiguration WithInstance<T>( this ContainerConfiguration @this, T instance, string name = null ) => @this.WithProvider( new InstanceExportDescriptorProvider<T>( instance, name ) );

		public static object Registered( this LifetimeContext @this, object instance )
		{
			instance.As<IDisposable>( @this.AddBoundInstance );
			return instance;
		}
	}
}