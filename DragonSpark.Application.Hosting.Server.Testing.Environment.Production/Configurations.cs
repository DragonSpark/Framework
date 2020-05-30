using DragonSpark.Composition;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Hosting.Server.Testing.Environment.Production
{
	public sealed class Configurations : IServiceConfiguration
	{
		[UsedImplicitly]
		public static Configurations Default { get; } = new Configurations();

		Configurations() {}

		public void Execute(IServiceCollection parameter)
		{
			parameter.Start<IDependency>()
			         .Forward<Dependency>()
			         .Singleton();
		}
	}
}