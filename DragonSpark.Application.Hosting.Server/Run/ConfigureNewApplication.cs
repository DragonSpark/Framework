using DragonSpark.Application.AspNet.Run;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System;

namespace DragonSpark.Application.Hosting.Server.Run;

public sealed record ConfigureNewApplication(Func<IHost, IApplication> New, ICommand<IApplication> Configure)
	: AspNet.Run.ConfigureNewApplication(New, Configure)
{
	public ConfigureNewApplication(ICommand<IApplication> Configure)
		: this(x => new Application(x.To<WebApplication>()), Configure) {}
}