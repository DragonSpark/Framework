using DragonSpark.Application.AspNet.Compose;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DragonSpark.Application.Hosting.Server;

public sealed class ServerApplicationProfile : ApplicationProfile
{
	public static ServerApplicationProfile Default { get; } = new();

	ServerApplicationProfile() : this(_ => {}, DefaultApplicationConfiguration.Default) {}

	public ServerApplicationProfile(Action<MvcOptions> configure, ICommand<IApplicationBuilder> application)
		: base(new DefaultServiceConfiguration(configure), application) {}
}