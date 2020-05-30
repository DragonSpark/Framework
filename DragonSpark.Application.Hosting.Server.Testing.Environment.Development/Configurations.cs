using DragonSpark.Composition;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Hosting.Server.Testing.Environment.Development
{
	public sealed class Configurations : IServiceConfiguration
	{
		public void Execute(IServiceCollection parameter)
		{
			parameter.Start<IDependency>()
			         .Forward<Dependency>()
			         .Singleton();
		}
	}
}