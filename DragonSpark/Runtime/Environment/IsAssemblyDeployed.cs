using System.Reflection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Reflection;

namespace DragonSpark.Runtime.Environment
{
	sealed class IsAssemblyDeployed : AnyCondition<Assembly>
	{
		public static IsAssemblyDeployed Default { get; } = new IsAssemblyDeployed();

		IsAssemblyDeployed() : this(I<AssemblyFileExists>.Default) {}

		public IsAssemblyDeployed(I<AssemblyFileExists> infer)
			: base(infer.From(ExecutableRuntimeFile.Default),
			       DevelopmentRuntimeFile.Default
			                             .To(I<AssemblyFileExists>.Default)
			                             .Then()
			                             .Inverse()
			                             .Get()) {}
	}
}