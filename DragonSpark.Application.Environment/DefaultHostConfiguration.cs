using DragonSpark.Composition.Compose;
using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Application.Environment;

public sealed class DefaultHostConfiguration : IHostConfiguration
{
	[UsedImplicitly]
	public static DefaultHostConfiguration Default { get; } = new();

	DefaultHostConfiguration() {}

	public void Execute(IHostBuilder parameter) {}
}