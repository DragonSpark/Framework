using DragonSpark.Reflection.Assemblies;
using DragonSpark.Runtime.Environment;
using Humanizer;
using Serilog.Core.Enrichers;

namespace DragonSpark.Diagnostics;

sealed class AssemblyDeployInformationEnricher : PropertyEnricher
{
	public static AssemblyDeployInformationEnricher Default { get; } = new();

	AssemblyDeployInformationEnricher() : this(PrimaryAssemblyDeployInformation.Default) {}

	public AssemblyDeployInformationEnricher(AssemblyDeployInformation value)
		: base(nameof(AssemblyDeployInformation).Humanize(), value, true) {}
}