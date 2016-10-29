using DragonSpark.Sources;
using System;

namespace DragonSpark.Activation.Location
{
	public sealed class GlobalServiceProvider : Scope<IServiceProvider>
	{
		public static IScope<IServiceProvider> Default { get; } = new GlobalServiceProvider();
		GlobalServiceProvider() : base( () => DefaultServices.Default ) {}
	}
}