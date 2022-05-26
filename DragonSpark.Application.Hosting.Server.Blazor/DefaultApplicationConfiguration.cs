using DragonSpark.Model.Commands;
using DragonSpark.Server.Requests.Warmup;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;

namespace DragonSpark.Application.Hosting.Server.Blazor;

sealed class DefaultApplicationConfiguration : ICommand<IApplicationBuilder>
{
	readonly StaticFileOptions             _files;
	readonly Action<IEndpointRouteBuilder> _endpoints;

	public DefaultApplicationConfiguration(StaticFileOptions files)
		: this(files, EndpointConfiguration.Default.Execute) {}

	public DefaultApplicationConfiguration(StaticFileOptions files, Action<IEndpointRouteBuilder> endpoints)
	{
		_files     = files;
		_endpoints = endpoints;
	}

	public void Execute(IApplicationBuilder parameter)
	{
		parameter.UseWarmupAwareHttpsRedirection()
		         .UseStaticFiles(_files)
		         .UseRouting()
		         .UseAuthentication()
		         .UseAuthorization()
		         .UseEndpoints(_endpoints);
	}
}