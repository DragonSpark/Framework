using AspNet.Security.OAuth.Coinbase;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Identity.Coinbase
{
	sealed class ConfigureApplication : ICommand<AuthenticationBuilder>
	{
		readonly Action<CoinbaseAuthenticationOptions> _configure;

		public ConfigureApplication(Action<CoinbaseAuthenticationOptions> configure) => _configure = configure;

		public void Execute(AuthenticationBuilder parameter)
		{
			var settings = parameter.Services.Deferred<CoinbaseApplicationSettings>();
			parameter.AddCoinbase(new ConfigureAuthentication(settings, _configure).Execute);
			parameter.Services.Register<CoinbaseApplicationSettings>();
		}
	}
}