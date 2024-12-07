using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;

namespace DragonSpark.Application.Compose;

sealed class ServerConfiguration : ICommand<IWebHostBuilder>
{
	readonly Action<IApplicationBuilder> _configure;

	public ServerConfiguration(ICommand<IApplicationBuilder> configure) : this(configure.Execute) {}

	public ServerConfiguration(Action<IApplicationBuilder> configure) => _configure = configure;

	public void Execute(IWebHostBuilder parameter)
	{
		parameter.Configure(_configure);
	}
}