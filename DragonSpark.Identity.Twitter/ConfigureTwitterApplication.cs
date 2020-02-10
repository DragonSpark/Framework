using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Identity.Twitter
{
	sealed class ConfigureTwitterApplication : ICommand<IServiceCollection>
	{
		public static ConfigureTwitterApplication Default { get; } = new ConfigureTwitterApplication();

		ConfigureTwitterApplication() {}

		public void Execute(IServiceCollection parameter)
		{
			parameter.Register<TwitterApplicationSettings>()
			         .AddAuthentication()
			         .Pair(parameter.Deferred<TwitterApplicationSettings>())
			         .With((x, y) => x.AddTwitter(new ConfigureTwitterAuthentication(y).Execute));
		}
	}
}