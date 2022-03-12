using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Composition;

sealed class DetermineModularity : Select<HostBuilderContext, Modularity>
{
	public static DetermineModularity Default { get; } = new();

	DetermineModularity() : base(GetHostEnvironmentName.Default.Then().Select(CreateModularity.Default)) {}
}