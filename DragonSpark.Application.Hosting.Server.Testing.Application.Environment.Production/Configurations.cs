using DragonSpark.Composition;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Hosting.Server.Testing.Application.Environment.Production
{
	public sealed class Configurations : IServiceConfiguration
	{
		public static Configurations Default { get; } = new Configurations();

		Configurations() {}

		public void Execute(IServiceCollection parameter)
		{
			parameter.For<IDependency>()
			         .Map<Dependency>()
			         .Singleton();
		}
	}

	public sealed class Dependency : IDependency {}
}