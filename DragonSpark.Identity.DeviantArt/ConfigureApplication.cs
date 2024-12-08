using AspNet.Security.OAuth.DeviantArt;
using DragonSpark.Application.AspNet.Security.Identity.Claims.Actions;
using DragonSpark.Composition;
using DragonSpark.Identity.DeviantArt.Claims;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Identity.DeviantArt;

sealed class ConfigureApplication : ICommand<AuthenticationBuilder>
{
	public static ConfigureApplication Default { get; } = new();

	ConfigureApplication() : this(DefaultClaimActions.Default, _ => {}) {}

	readonly IClaimAction                            _claims;
	readonly Action<DeviantArtAuthenticationOptions> _configure;

	public ConfigureApplication(Action<DeviantArtAuthenticationOptions> configure)
		: this(DefaultClaimActions.Default, configure) {}

	public ConfigureApplication(IClaimAction claims, Action<DeviantArtAuthenticationOptions> configure)
	{
		_claims    = claims;
		_configure = configure;
	}

	public void Execute(AuthenticationBuilder parameter)
	{
		var settings = parameter.Services.Deferred<DeviantArtApplicationSettings>();
		parameter.AddDeviantArt(new ConfigureAuthentication(settings, _claims, _configure).Execute);
		parameter.Services.Register<DeviantArtApplicationSettings>();
	}
}