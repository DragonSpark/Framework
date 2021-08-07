using AspNet.Security.OAuth.Patreon;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Identity.Patreon
{
	sealed class ConfigureApplication : ICommand<AuthenticationBuilder>
	{
		readonly Action<PatreonAuthenticationOptions> _configure;

		public ConfigureApplication(Action<PatreonAuthenticationOptions> configure) => _configure = configure;

		public void Execute(AuthenticationBuilder parameter)
		{
			var settings = parameter.Services.Deferred<PatreonApplicationSettings>();
			parameter.AddPatreon(new ConfigureAuthentication(settings, _configure).Execute);
			parameter.Services.Register<PatreonApplicationSettings>();
		}
	}
}