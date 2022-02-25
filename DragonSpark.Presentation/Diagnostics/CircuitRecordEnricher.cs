using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Presentation.Connections.Circuits;
using Serilog.Core;
using Serilog.Events;

namespace DragonSpark.Presentation.Diagnostics;

sealed class CircuitRecordEnricher : ILogEventEnricher
{
	readonly IConnectionIdentifier         _identifier;
	readonly ITable<string, CircuitRecord> _records;

	public CircuitRecordEnricher(IConnectionIdentifier identifier)
		: this(identifier, CircuitRecordIdentification.Default) {}

	public CircuitRecordEnricher(IConnectionIdentifier identifier, ITable<string, CircuitRecord> records)
	{
		_identifier = identifier;
		_records    = records;
	}

	public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
	{
		var identifier = _identifier.Get();
		if (!string.IsNullOrEmpty(identifier))
		{
			{
				var property = propertyFactory.CreateProperty(nameof(ConnectionIdentifier), identifier);
				logEvent.AddPropertyIfAbsent(property);
			}

			if (_records.IsSatisfiedBy(identifier))
			{
				var record = _records.Get(identifier);
				{
					if (record.User.Identity?.IsAuthenticated ?? false)
					{
						var property = propertyFactory.CreateProperty("CircuitUserName", record.User.Identity.Name);
						logEvent.AddPropertyIfAbsent(property);
					}
				}
				{
					var path     = $"/{record.Navigation.ToBaseRelativePath(record.Navigation.Uri)}";
					var property = propertyFactory.CreateProperty("CircuitRequestPath", path);
					logEvent.AddPropertyIfAbsent(property);
				}
			}
		}
	}
}