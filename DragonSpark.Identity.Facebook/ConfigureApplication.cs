using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Identity.Facebook
{
	sealed class ConfigureApplication : ICommand<AuthenticationBuilder>
	{
		readonly Action<FacebookOptions> _configure;

		public ConfigureApplication(Action<FacebookOptions> configure) => _configure = configure;

		public void Execute(AuthenticationBuilder parameter)
		{
			var settings = parameter.Services.Deferred<FacebookApplicationSettings>();
			parameter.AddFacebook(new ConfigureAuthentication(settings, _configure).Execute);
			parameter.Services.Register<FacebookApplicationSettings>();
		}
	}
}