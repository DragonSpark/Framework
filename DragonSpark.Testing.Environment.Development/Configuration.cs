using DragonSpark.Composition;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Testing.Environment.Development;

public sealed class Configuration : IServiceConfiguration
{
	[UsedImplicitly]
	public static Configuration Default { get; } = new();

	Configuration() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.AddSingleton("Hello World!  Configured from Development.");
	}
}