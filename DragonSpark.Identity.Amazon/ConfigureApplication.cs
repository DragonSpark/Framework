using AspNet.Security.OAuth.Amazon;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Identity.Amazon
{
	sealed class ConfigureApplication : ICommand<AuthenticationBuilder>
	{
		readonly Action<AmazonAuthenticationOptions> _configure;

		public ConfigureApplication(Action<AmazonAuthenticationOptions> configure) => _configure = configure;

		public void Execute(AuthenticationBuilder parameter)
		{
			var settings = parameter.Services.Deferred<AmazonApplicationSettings>();
			parameter.AddAmazon(new ConfigureAuthentication(settings, _configure).Execute);
			parameter.Services.Register<AmazonApplicationSettings>();
		}
	}
}