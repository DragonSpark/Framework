using Serilog.Configuration;

namespace DragonSpark.Diagnostics.Configurations
{
	public class EnrichWithPropertyCommand : EnrichCommandBase
	{
		public string PropertyName { get; set; }

		public object Value { get; set; }

		public bool DestructureObjects { get; set; }

		protected override void Configure( LoggerEnrichmentConfiguration configuration ) => configuration.WithProperty( PropertyName, Value, DestructureObjects );
	}
}