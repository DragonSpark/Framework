using DragonSpark.Composition.Compose;
using JetBrains.Annotations;

namespace DragonSpark.Testing.Environment.Platform.Development;

public sealed class DefaultHostConfiguration : IHostConfiguration
{
	[UsedImplicitly]
	public static DefaultHostConfiguration Default { get; } = new();

	DefaultHostConfiguration() {}

	public void Execute(HostingInput parameter) {}
}