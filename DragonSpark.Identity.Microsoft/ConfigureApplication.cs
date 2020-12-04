using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Identity.Microsoft
{
	sealed class ConfigureApplication : ICommand<AuthenticationBuilder>
	{
		public static ConfigureApplication Default { get; } = new ConfigureApplication();

		ConfigureApplication() {}

		public void Execute(AuthenticationBuilder parameter)
		{
			parameter.Services.Register<MicrosoftApplicationSettings>()
			         .Return(parameter)
			         .Tuple(parameter.Services.Deferred<MicrosoftApplicationSettings>())
			         .With((x, y) => x.AddMicrosoftAccount(new ConfigureAuthentication(y).Execute))
			         .Return(parameter.Services)
			         ;
		}
	}
}