using DragonSpark.Composition;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Hosting.Server.Testing.Application.Environment.Development
{
	public sealed class Configurations : IServiceConfiguration
	{
		public void Execute(IServiceCollection parameter)
		{
			parameter.For<IDependency>()
			         .Map<Dependency>()
			         .Singleton();
		}
	}
}