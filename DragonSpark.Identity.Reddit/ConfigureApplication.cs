using AspNet.Security.OAuth.Reddit;
using DragonSpark.Application.Security.Identity.Claims.Actions;
using DragonSpark.Composition;
using DragonSpark.Identity.Reddit.Claims;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Identity.Reddit;

sealed class ConfigureApplication : ICommand<AuthenticationBuilder>
{
	public static ConfigureApplication Default { get; } = new();

	ConfigureApplication() : this(DefaultClaimActions.Default, _ => {}) {}

	readonly IClaimAction                        _claims;
	readonly Action<RedditAuthenticationOptions> _configure;

	public ConfigureApplication(Action<RedditAuthenticationOptions> configure)
		: this(DefaultClaimActions.Default, configure) {}

	public ConfigureApplication(IClaimAction claims, Action<RedditAuthenticationOptions> configure)
	{
		_claims    = claims;
		_configure = configure;
	}

	public void Execute(AuthenticationBuilder parameter)
	{
		var settings = parameter.Services.Deferred<RedditApplicationSettings>();
		parameter.AddReddit(new ConfigureAuthentication(settings, _claims, _configure).Execute);
		parameter.Services.Register<RedditApplicationSettings>();
	}
}