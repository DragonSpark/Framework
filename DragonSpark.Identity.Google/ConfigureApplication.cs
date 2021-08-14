using DragonSpark.Application.Security.Identity.Claims.Actions;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Identity.Google.Claims;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Identity.Google
{
	sealed class ConfigureApplication : ICommand<AuthenticationBuilder>
	{
		public static ConfigureApplication Default { get; } = new ConfigureApplication();

		ConfigureApplication() : this(DefaultClaimActions.Default) {}

		readonly IClaimAction _claims;

		public ConfigureApplication(IClaimAction claims) => _claims = claims;

		public void Execute(AuthenticationBuilder parameter)
		{
			var settings = parameter.Services.Deferred<GoogleApplicationSettings>();
			parameter.Services.Register<GoogleApplicationSettings>()
			         .Return(parameter)
			         .AddGoogle(new ConfigureAuthentication(settings, _claims).Execute)
			         ;
		}
	}
}