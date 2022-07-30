using Serilog.Core;
using Serilog.Events;

namespace DragonSpark.Presentation.Diagnostics;

sealed class ReferrerEnricher : ILogEventEnricher
{
	readonly CurrentReferrer _referrer;

	public ReferrerEnricher(CurrentReferrer referrer) => _referrer = referrer;

	public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
	{
		var referrer = _referrer.Get();
		if (referrer is not null)
		{
			var property = propertyFactory.CreateProperty("CircuitReferrer", referrer);
			logEvent.AddPropertyIfAbsent(property);
		}
	}
}