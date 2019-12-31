using DragonSpark.Compose;
using DragonSpark.Compose.Extents;
using DragonSpark.Model.Selection.Conditions;
using System.Reflection;

namespace DragonSpark.Runtime.Environment
{
	sealed class IsAssemblyDeployed : AnyCondition<Assembly>
	{
		public static IsAssemblyDeployed Default { get; } = new IsAssemblyDeployed();

		IsAssemblyDeployed() : this(Start.An.Extent<AssemblyFileExists>()) {}

		public IsAssemblyDeployed(Extent<AssemblyFileExists> infer)
			: base(infer.From(ExecutableRuntimeFile.Default),
			       infer.From(DevelopmentRuntimeFile.Default)
			            .Then()
			            .Inverse()
			            .Get()) {}
	}
}