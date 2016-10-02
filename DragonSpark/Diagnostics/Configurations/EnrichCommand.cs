using DragonSpark.Extensions;
using DragonSpark.Runtime;
using Serilog.Configuration;
using Serilog.Core;
using System.Windows.Markup;

namespace DragonSpark.Diagnostics.Configurations
{
	[ContentProperty( nameof(Items) )]
	public class EnrichCommand : EnrichCommandBase
	{
		public DeclarativeCollection<ILogEventEnricher> Items { get; } = new DeclarativeCollection<ILogEventEnricher>();
		
		protected override void Configure( LoggerEnrichmentConfiguration configuration ) => configuration.With( Items.Fixed() );
	}
}