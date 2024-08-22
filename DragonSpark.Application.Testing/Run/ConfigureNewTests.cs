using DragonSpark.Application.Hosting.Server;
using DragonSpark.Application.Run;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using Xunit;
using RunApplication = DragonSpark.Application.Hosting.Server.Run.RunApplication;

namespace DragonSpark.Application.Testing.Run;

public sealed class ConfigureNewTests
{
	[Fact]
	public void Verify()
	{
		new Program().Get([]);
	}

	sealed class Program : RunApplication
	{
		public Program() : base(NewHost.Default, ConfigureBuilder.Default, ConfigureApplication.Default) {}
	}

	sealed class NewHost : DragonSpark.Application.Hosting.Server.Run.NewHost
	{
		public static NewHost Default { get; } = new();

		NewHost()
			: base(Start.A.Host()
			            .WithDefaultComposition()
			            .RegisterModularity()
			            .WithAmbientConfiguration()
			            .WithFrameworkConfigurations()
			            //
			            .WithServerApplication()
			            .NamedFromPrimaryAssembly()
			            // .WithEnvironmentalConfiguration()
			            // .WithBearerSupport()
			            //
			            /*.WithIdentity<Entities.Identity.User>()
			            .StoredIn<ApplicationState>()
			            .As.Application()
			            .Using.Configuration(configuration)
			            //*/
			            //.WithIdentityClaimsRelay()
			            .WithAuthentication()
			            .Get()
			            .As.Application()
			            .WithInitializationLogging<Program>()) {}
	}

	sealed class ConfigureBuilder : Commands<IHostedApplicationBuilder>
	{
		public static ConfigureBuilder Default { get; } = new();

		ConfigureBuilder() : base(Configure.Instance) {}

		sealed class Configure : ICommand<IHostedApplicationBuilder>
		{
			public static Configure Instance { get; } = new();

			Configure() {}

			public void Execute(IHostedApplicationBuilder parameter)
			{
				// Add service defaults & Aspire components.
				parameter.AddServiceDefaults();

				// Add services to the container.
				parameter.Services.AddProblemDetails();
			}
		}
	}

	sealed class ConfigureApplication : Commands<IApplication>
	{
		public static ConfigureApplication Default { get; } = new();

		ConfigureApplication()
			: base(DefaultConfiguration.Instance) {}

		sealed class DefaultConfiguration : ICommand<IApplication>
		{
			public static DefaultConfiguration Instance { get; } = new();

			DefaultConfiguration() {}

			public void Execute(IApplication parameter)
			{
				// Configure the HTTP request pipeline.
				parameter.UseExceptionHandler();

				parameter.MapGet("/weatherforecast", WeatherForecastService.Default.Get);

				parameter.MapDefaultEndpoints();
			}
		}
	}

	sealed class WeatherForecastService
	{
		public static WeatherForecastService Default { get; } = new();

		WeatherForecastService()
			: this("Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering",
			       "Scorching") {}

		readonly string[] _summaries;

		public WeatherForecastService(params string[] summaries) => _summaries = summaries;

		public WeatherForecast[] Get()
			=> Enumerable.Range(1, 5)
			             .Select(x => new WeatherForecast
				                     (DateOnly.FromDateTime(DateTime.Now.AddDays(x)),
				                      Random.Shared.Next(-20, 55),
				                      _summaries[Random.Shared.Next(_summaries.Length)]))
			             .ToArray();
	}

	record WeatherForecast([UsedImplicitly] DateOnly Date, int TemperatureC, string? Summary)
	{
		[UsedImplicitly]
		public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
	}
}

public static class Extensions
{
	public static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder @this)
		=> @this.AddDefaultHealthChecks()
		        .Services.AddServiceDiscovery()
		        .ConfigureHttpClientDefaults(x =>
		                                     {
			                                     // Turn on resilience by default
			                                     x.AddStandardResilienceHandler();

			                                     // Turn on service discovery by default
			                                     x.AddServiceDiscovery();
		                                     })
		        .Return(@this);

	static IHostApplicationBuilder AddDefaultHealthChecks(this IHostApplicationBuilder @this)
		=> @this.Services.AddHealthChecks()
		        // Add a default liveness check to ensure app is responsive
		        .AddCheck("self", static () => HealthCheckResult.Healthy(), ["live"])
		        .Return(@this);

	public static IApplication MapDefaultEndpoints(this IApplication @this)
	{
		// Adding health checks endpoints to applications in non-development environments has security implications.
		// See https://aka.ms/dotnet/aspire/healthchecks for details before enabling these endpoints in non-development environments.
		if (@this.Environment.IsDevelopment())
		{
			// All health checks must pass for app to be considered ready to accept traffic after starting
			@this.MapHealthChecks("/health");

			// Only health checks tagged with the "live" tag must pass for app to be considered alive
			@this.MapHealthChecks("/alive", new() { Predicate = r => r.Tags.Contains("live") });
		}

		return @this;
	}
}