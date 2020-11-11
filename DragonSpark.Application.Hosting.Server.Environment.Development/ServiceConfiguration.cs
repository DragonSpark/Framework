using DragonSpark.Composition;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Hosting.Server.Environment.Development
{
	public sealed class ServiceConfiguration : IServiceConfiguration
	{
		public static ServiceConfiguration Default { get; } = new ServiceConfiguration();

		ServiceConfiguration() {}

		public void Execute(IServiceCollection parameter)
		{
			parameter.AddDatabaseDeveloperPageExceptionFilter();
		}
	}
}