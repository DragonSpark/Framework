using DragonSpark.Presentation.Connections.Circuits;
using Serilog.Core;
using Serilog.Events;

namespace DragonSpark.Presentation.Diagnostics;

sealed class CircuitRecordEnricher : ILogEventEnricher
{
	public static CircuitRecordEnricher Default { get; } = new();

	CircuitRecordEnricher() : this(CurrentCircuit.Default) {}

	readonly CurrentCircuit _current;

	public CircuitRecordEnricher(CurrentCircuit current) => _current = current;

	public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
	{
		var current = _current.Get();

		if (current is not null)
		{
			var (_, navigation, user) = current;
			if (user.Identity?.IsAuthenticated ?? false)
			{
				var property = propertyFactory.CreateProperty("CircuitUserName", user.Identity.Name);
				logEvent.AddPropertyIfAbsent(property);
			}

			{
				var path     = $"/{navigation.ToBaseRelativePath(navigation.Uri)}";
				var property = propertyFactory.CreateProperty("CircuitRequestPath", path);
				logEvent.AddPropertyIfAbsent(property);
			}
			{
				var property = propertyFactory.CreateProperty("CircuitRequestBaseUri", navigation.BaseUri);
				logEvent.AddPropertyIfAbsent(property);
			}
		}
	}
}