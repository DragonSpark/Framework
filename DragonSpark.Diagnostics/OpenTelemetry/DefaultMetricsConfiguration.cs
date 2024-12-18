using DragonSpark.Model.Commands;
using DragonSpark.Model.Sequences;
using OpenTelemetry.Metrics;

namespace DragonSpark.Diagnostics.OpenTelemetry;

sealed class DefaultMetricsConfiguration : Command<MeterProviderBuilder>
{
	public DefaultMetricsConfiguration(Array<string> sources)
        : base(x => x.AddHttpClientInstrumentation().AddRuntimeInstrumentation().AddMeter(sources)) {}
}
