using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Identity.Google
{
	sealed class ConfigureApplication : ICommand<AuthenticationBuilder>
	{
		public static ConfigureApplication Default { get; } = new ConfigureApplication();

		ConfigureApplication() {}

		public void Execute(AuthenticationBuilder parameter)
		{
			parameter.Services.Register<GoogleApplicationSettings>()
			         .Return(parameter)
			         .Tuple(parameter.Services.Deferred<GoogleApplicationSettings>())
			         .With((x, y) => x.AddGoogle(new ConfigureAuthentication(y).Execute))
			         .Return(parameter.Services)
			         ;
		}
	}
}