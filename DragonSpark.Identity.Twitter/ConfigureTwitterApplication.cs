using DragonSpark.Application.Security.Identity.Claims.Actions;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Identity.Twitter
{
	sealed class ConfigureTwitterApplication : ICommand<AuthenticationBuilder>
	{
		readonly IClaimAction _action;

		public ConfigureTwitterApplication(IClaimAction action) => _action = action;

		public void Execute(AuthenticationBuilder parameter)
		{
			var settings       = parameter.Services.Deferred<TwitterApplicationSettings>();
			var authentication = new ConfigureTwitterAuthentication(settings, _action);
			parameter.Services.Register<TwitterApplicationSettings>()
			         .Return(parameter)
			         .AddTwitter(authentication.Execute)
				;
		}
	}
}