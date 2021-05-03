using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Identity.PayPal
{
	sealed class ConfigureApplication : ICommand<AuthenticationBuilder>
	{
		public static ConfigureApplication Default { get; } = new ConfigureApplication();

		ConfigureApplication() {}

		public void Execute(AuthenticationBuilder parameter)
		{
			parameter.Services.Register<PayPalApplicationSettings>()
			         .Return(parameter)
			         .Tuple(parameter.Services.Deferred<PayPalApplicationSettings>())
			         .With((x, y) => x.AddPaypal(new ConfigureAuthentication(y).Execute));
		}
	}
}