using DragonSpark.Extensions;
using DragonSpark.Sources;
using System;

namespace DragonSpark.Activation.Location
{
	public sealed class GlobalServiceProvider : Scope<IServiceProvider>
	{
		public static IScope<IServiceProvider> Default { get; } = new GlobalServiceProvider();

		GlobalServiceProvider() : base( () => DefaultServices.Default ) {}

		public static T GetService<T>() => GetService<T>( typeof(T) );

		public static T GetService<T>( Type type ) => Default.Get().Get<T>( type );
	}
}