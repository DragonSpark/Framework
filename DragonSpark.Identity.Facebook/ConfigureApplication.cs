using DragonSpark.Application.Security.Identity.Claims;
using DragonSpark.Composition;
using DragonSpark.Identity.Facebook.Claims;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Identity.Facebook
{
	sealed class ConfigureApplication : ICommand<AuthenticationBuilder>
	{
		public static ConfigureApplication Default { get; } = new ConfigureApplication();

		ConfigureApplication() : this(DefaultClaimActions.Default) {}

		readonly IClaimAction _claims;

		public ConfigureApplication(IClaimAction claims) => _claims = claims;

		public void Execute(AuthenticationBuilder parameter)
		{
			var settings = parameter.Services.Deferred<FacebookApplicationSettings>();
			parameter.AddFacebook(new ConfigureAuthentication(settings, _claims).Execute);
			parameter.Services.Register<FacebookApplicationSettings>();
		}
	}
}