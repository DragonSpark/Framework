using DragonSpark.Model.Selection.Alterations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace DragonSpark.Application.Compose;

sealed class WebHostConfiguration : IAlteration<IHostBuilder>
{
	readonly Action<IWebHostBuilder> _configure;

	public WebHostConfiguration(Action<IWebHostBuilder> configure) => _configure = configure;

	public IHostBuilder Get(IHostBuilder parameter) => parameter.ConfigureWebHost(_configure);
}