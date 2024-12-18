using DragonSpark.Application.Mobile.Diagnostics;
using DragonSpark.Application.Mobile.Run;
using DragonSpark.Composition.Compose;

namespace DragonSpark.Application.Mobile;

public static class Extensions
{
	public static BuildHostContext WithFrameworkConfigurations(this BuildHostContext @this)
		=> Configure.Default.Get(@this);

	public static IRunApplication ForSynchronousExecution(this IRunApplication @this)
		=> new SynchronousAwareRunApplication(@this);

	public static IRunApplication WithEnvironmentalConfigurations(this IRunApplication @this)
		=> new EnvironmentalConfigurationAwareRunApplication(@this);

	public static BuildHostContext WithOpenTelemetry(this BuildHostContext @this, params string[] sources)
		=> @this.Configure(new ConfigureOpenTelemetry(sources));
}
