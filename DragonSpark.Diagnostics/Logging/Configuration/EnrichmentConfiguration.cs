using DragonSpark.Model.Sequences;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;

namespace DragonSpark.Diagnostics.Logging.Configuration
{
	public class EnrichmentConfiguration : ILoggingEnrichmentConfiguration
	{
		readonly Array<ILogEventEnricher> _elements;

		public EnrichmentConfiguration(params ILogEventEnricher[] elements) : this(elements.Result()) {}

		public EnrichmentConfiguration(Array<ILogEventEnricher> elements) => _elements = elements;

		public LoggerConfiguration Get(LoggerEnrichmentConfiguration parameter) => parameter.With(_elements);
	}
}