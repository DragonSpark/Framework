using DragonSpark.Model.Selection.Stores;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Composition;

sealed class ModularityComponents : ReferenceValueStore<IHostEnvironment, Modularity>
{
	public static ModularityComponents Default { get; } = new();

	ModularityComponents() : base(CreateModularityComponents.Default) {}
}