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
			var settings = parameter.Services.Deferred<MicrosoftApplicationSettings>();
			parameter.Services.Register<MicrosoftApplicationSettings>()
			         .Return(parameter)
			         .AddMicrosoftAccount(new ConfigureAuthentication(settings).Execute)
			         ;
		}
	}
}