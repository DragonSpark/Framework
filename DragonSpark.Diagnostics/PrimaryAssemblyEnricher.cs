using DragonSpark.Reflection.Assemblies;
using DragonSpark.Runtime.Environment;
using Humanizer;
using Serilog.Core.Enrichers;

namespace DragonSpark.Diagnostics;

sealed class PrimaryAssemblyEnricher : PropertyEnricher
{
	public static PrimaryAssemblyEnricher Default { get; } = new();

	PrimaryAssemblyEnricher() : this(PrimaryAssemblyDetails.Default) {}

	public PrimaryAssemblyEnricher(AssemblyDetails value) : base(nameof(PrimaryAssembly).Humanize(), value, true) {}
}