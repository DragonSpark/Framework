using DragonSpark.Model.Sequences;

namespace DragonSpark.Diagnostics.OpenTelemetry;

public sealed class DefaultOpenTelemetryConfiguration : OpenTelemetryConfiguration
{
	public DefaultOpenTelemetryConfiguration(Array<string> sources)
		: base(new DefaultMetricsConfiguration(sources).Execute, new DefaultTracingConfiguration(sources).Execute) {}
}
