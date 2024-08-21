using DragonSpark.Application.Run;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Application.Hosting.Server.Run;

public sealed class Application : IApplication
{
	readonly WebApplication        _previous;
	readonly IApplicationBuilder   _builder;
	readonly IEndpointRouteBuilder _endpoint;

	public Application(WebApplication previous) : this(previous, previous, previous) {}

	public Application(WebApplication previous, IApplicationBuilder builder, IEndpointRouteBuilder endpoint)
	{
		_previous = previous;
		_builder  = builder;
		_endpoint = endpoint;
	}

	public Task StartAsync(CancellationToken cancellationToken = new()) => _previous.StartAsync(cancellationToken);

	public Task StopAsync(CancellationToken cancellationToken = new()) => _previous.StopAsync(cancellationToken);

	public IServiceProvider Services => _previous.Services;

	public IApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware) => _previous.Use(middleware);

	public IApplicationBuilder New() => _builder.New();

	public RequestDelegate Build() => _builder.Build();

	public IServiceProvider ApplicationServices
	{
		get => _builder.ApplicationServices;
		set => _builder.ApplicationServices = value;
	}

	public IFeatureCollection ServerFeatures => _builder.ServerFeatures;

	public IDictionary<string, object?> Properties => _builder.Properties;

	public ValueTask DisposeAsync() => _previous.DisposeAsync();

	public IApplicationBuilder CreateApplicationBuilder() => _endpoint.CreateApplicationBuilder();

	public IServiceProvider ServiceProvider => _endpoint.ServiceProvider;

	public ICollection<EndpointDataSource> DataSources => _endpoint.DataSources;

	public IConfiguration Configuration => _previous.Configuration;

	public IHostEnvironment Environment => _previous.Environment;

	public IHostApplicationLifetime Lifetime => _previous.Lifetime;

	public ILogger Logger => _previous.Logger;

	public ICollection<string> Urls => _previous.Urls;

	public void Dispose()
	{
		((IDisposable)_previous).Dispose();
	}
}