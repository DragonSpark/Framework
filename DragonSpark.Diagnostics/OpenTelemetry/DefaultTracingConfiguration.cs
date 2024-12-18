using DragonSpark.Model.Commands;
using DragonSpark.Model.Sequences;
using OpenTelemetry.Trace;

namespace DragonSpark.Diagnostics.OpenTelemetry;

sealed class DefaultTracingConfiguration : Command<TracerProviderBuilder>
{
	public DefaultTracingConfiguration(Array<string> sources) 
        : base(x => x.AddHttpClientInstrumentation().AddSource(sources)) {}
}
