using DragonSpark.Model.Results;
using DragonSpark.Reflection.Assemblies;

namespace DragonSpark.Runtime.Environment;

public sealed class PrimaryAssemblyDeployInformation : Instance<AssemblyDeployInformation>
{
	public static PrimaryAssemblyDeployInformation Default { get; } = new();

	PrimaryAssemblyDeployInformation() : base(GetAssemblyDeployInformation.Default.Get(PrimaryAssembly.Default)) {}
}