using DragonSpark.Compose;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Hosting.Server.Run;

sealed class WebHostBuilder : IWebHostBuilder, ISupportsStartup
{
	readonly WebApplication _application;

	public WebHostBuilder(WebApplication application) => _application = application;

	public IWebHost Build() => throw new InvalidOperationException();

	public IWebHostBuilder ConfigureAppConfiguration(
		Action<WebHostBuilderContext, IConfigurationBuilder> configureDelegate) => this;

	public IWebHostBuilder ConfigureServices(Action<IServiceCollection> configureServices) => this;

	public IWebHostBuilder ConfigureServices(Action<WebHostBuilderContext, IServiceCollection> configureServices)
		=> this;

	public string? GetSetting(string key) => null;

	public IWebHostBuilder UseSetting(string key, string? value)
	{
		if (key == WebHostDefaults.ApplicationKey)
		{
			_application.Environment.ApplicationName = value.Verify();
		}

		return this;
	}

	public IWebHostBuilder Configure(Action<IApplicationBuilder> configure)
	{
		configure(_application);
		return this;
	}

	public IWebHostBuilder Configure(Action<WebHostBuilderContext, IApplicationBuilder> configure)
	{
		return this;
	}

	public IWebHostBuilder UseStartup(Type startupType) => throw new InvalidOperationException();

	public IWebHostBuilder UseStartup<TStartup>(Func<WebHostBuilderContext, TStartup> startupFactory)
		=> throw new InvalidOperationException();
}