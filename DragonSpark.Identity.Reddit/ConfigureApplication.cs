using DragonSpark.Application.Security.Identity.Claims.Actions;
using DragonSpark.Composition;
using DragonSpark.Identity.Reddit.Claims;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Identity.Reddit
{
	sealed class ConfigureApplication : ICommand<AuthenticationBuilder>
	{
		public static ConfigureApplication Default { get; } = new ConfigureApplication();

		ConfigureApplication() : this(DefaultClaimActions.Default) {}

		readonly IClaimAction _claims;

		public ConfigureApplication(IClaimAction claims) => _claims = claims;

		public void Execute(AuthenticationBuilder parameter)
		{
			var settings = parameter.Services.Deferred<RedditApplicationSettings>();
			parameter.AddReddit(new ConfigureAuthentication(settings, _claims).Execute);
			parameter.Services.Register<RedditApplicationSettings>();
		}
	}
}