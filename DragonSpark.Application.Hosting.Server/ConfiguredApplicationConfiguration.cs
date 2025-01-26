using DragonSpark.Model.Commands;
using DragonSpark.Server.Requests.Warmup;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;

namespace DragonSpark.Application.Hosting.Server;

public sealed class ConfiguredApplicationConfiguration : ICommand<IApplicationBuilder>
{
	public static ConfiguredApplicationConfiguration Default { get; } = new();

	ConfiguredApplicationConfiguration() : this(EndpointConfiguration.Default.Execute) {}

	readonly Action<IEndpointRouteBuilder> _endpoints;

	public ConfiguredApplicationConfiguration(Action<IEndpointRouteBuilder> endpoints) => _endpoints = endpoints;

	public void Execute(IApplicationBuilder parameter)
	{
		parameter.UseWarmupAwareHttpsRedirection()
		         .UseAuthentication()
		         .UseRouting()
		         .UseCors()
		         .UseAuthorization()
				 .UseOutputCache()
		         .UseEndpoints(_endpoints);
	}
}
