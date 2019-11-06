using Humanizer;
using Serilog.Core.Enrichers;
using DragonSpark.Reflection.Assemblies;
using DragonSpark.Runtime.Environment;

namespace DragonSpark.Diagnostics.Logging.Enrichment
{
	sealed class PrimaryAssemblyEnricher : PropertyEnricher
	{
		public static PrimaryAssemblyEnricher Default { get; } = new PrimaryAssemblyEnricher();

		PrimaryAssemblyEnricher() : this(PrimaryAssemblyDetails.Default.Get()) {}

		public PrimaryAssemblyEnricher(AssemblyDetails value) : base(nameof(PrimaryAssembly).Humanize(), value, true) {}
	}
}