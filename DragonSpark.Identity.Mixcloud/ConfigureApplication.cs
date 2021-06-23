﻿using AspNet.Security.OAuth.Mixcloud;
using DragonSpark.Application.Security.Identity.Claims;
using DragonSpark.Composition;
using DragonSpark.Identity.Mixcloud.Claims;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;

namespace DragonSpark.Identity.Mixcloud
{
	sealed class ConfigureApplication : ICommand<AuthenticationBuilder>
	{
		public static ConfigureApplication Default { get; } = new ConfigureApplication();

		ConfigureApplication() : this(DefaultClaimActions.Default) {}

		readonly IClaimAction _claims;

		public ConfigureApplication(IClaimAction claims) => _claims = claims;

		public void Execute(AuthenticationBuilder parameter)
		{
			var settings = parameter.Services.Deferred<MixcloudApplicationSettings>();
			parameter.AddMixcloud(new ConfigureAuthentication(settings, _claims).Execute);
			parameter.Services.Register<MixcloudApplicationSettings>();
		}
	}
}