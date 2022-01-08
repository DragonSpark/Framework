using DragonSpark.Composition.Compose;
using JetBrains.Annotations;

namespace DragonSpark.Application.Environment;

public sealed class DefaultHostConfiguration : IHostConfiguration
{
	[UsedImplicitly]
	public static DefaultHostConfiguration Default { get; } = new();

	DefaultHostConfiguration() {}

	public void Execute(HostConfiguration parameter) {}
}