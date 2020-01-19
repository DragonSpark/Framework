using DragonSpark.Composition;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Testing.Environment.Development
{
	public sealed class Configuration : IServiceConfiguration
	{
		public static Configuration Default { get; } = new Configuration();

		Configuration() {}

		public void Execute(IServiceCollection parameter)
		{
			parameter.AddSingleton("Hello World!  Configured from Development.");
		}
	}
}