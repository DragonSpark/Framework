using DragonSpark.Application.Mobile.Diagnostics;
using DragonSpark.Composition.Compose;

namespace DragonSpark.Application.Mobile;

public static class Extensions
{
	public static BuildHostContext WithOpenTelemetry(this BuildHostContext @this, params string[] sources)
		=> @this.Configure(new ConfigureOpenTelemetry(sources));
}