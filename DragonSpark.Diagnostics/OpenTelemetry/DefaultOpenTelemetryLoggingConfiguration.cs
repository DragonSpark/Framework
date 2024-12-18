using DragonSpark.Model.Commands;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Diagnostics.OpenTelemetry;

public sealed class DefaultOpenTelemetryLoggingConfiguration : Command<ILoggingBuilder>
{
	public static DefaultOpenTelemetryLoggingConfiguration Default { get; } = new();

	DefaultOpenTelemetryLoggingConfiguration()
		: base(x => x.AddOpenTelemetry(y => y.IncludeFormattedMessage = y.IncludeScopes = true)) {}
}