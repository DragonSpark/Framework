using DragonSpark.Composition;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Hosting.Server.Testing.Environment
{
	public sealed class RootServiceConfiguration : IServiceConfiguration
	{
		public static RootServiceConfiguration Default { get; } = new RootServiceConfiguration();

		RootServiceConfiguration() {}

		public void Execute(IServiceCollection parameter) {}
	}
}