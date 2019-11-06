using DragonSpark.Runtime.Objects;
using System;
using System.Linq;

namespace DragonSpark.Diagnostics.Logging.Configuration
{
	public sealed class ProjectionsConfiguration : LoggerConfigurations
	{
		public static ProjectionsConfiguration Default { get; } = new ProjectionsConfiguration();

		ProjectionsConfiguration() : this(Projectors.Default,
		                                  KnownProjectors.Default
		                                                 .Select(x => x.Reference().Select(y => y.Key).ToArray())
		                                                 .Get()) {}

		public ProjectionsConfiguration(IProjectors projectors, params Type[] projectionTypes)
			: base(new EnrichmentConfiguration(new ProjectionEnricher(projectors)).ToConfiguration(),
			       new ScalarConfiguration(projectionTypes).ToConfiguration()) {}
	}
}