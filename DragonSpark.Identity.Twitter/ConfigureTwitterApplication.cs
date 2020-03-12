using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Identity.Twitter
{
	sealed class ConfigureTwitterApplication : ICommand<AuthenticationBuilder>
	{
		public static ConfigureTwitterApplication Default { get; } = new ConfigureTwitterApplication();

		ConfigureTwitterApplication() {}

		public void Execute(AuthenticationBuilder parameter)
		{
			parameter.Services.Register<TwitterApplicationSettings>()
			         .Return(parameter)
			         .Pair(parameter.Services.Deferred<TwitterApplicationSettings>())
			         .With((x, y) => x.AddTwitter(new ConfigureTwitterAuthentication(y).Execute));
		}
	}
}