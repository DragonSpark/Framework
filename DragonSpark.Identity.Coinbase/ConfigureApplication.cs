using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;

namespace DragonSpark.Identity.Coinbase
{
	sealed class ConfigureApplication : ICommand<AuthenticationBuilder>
	{
		public static ConfigureApplication Default { get; } = new ConfigureApplication();

		ConfigureApplication() {}

		public void Execute(AuthenticationBuilder parameter)
		{
			parameter.Services.Register<CoinbaseApplicationSettings>()
			         .Return(parameter)
			         ;
		}
	}
}