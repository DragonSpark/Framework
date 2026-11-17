using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System.Reflection;

namespace DragonSpark.Application.Hosting.Server.Blazor;

sealed class ApplyBlazorWebApplication<T> : ICommand<IApplicationBuilder>
{
	readonly string                                             _name;
	readonly ICommand<RazorComponentsEndpointConventionBuilder> _endpoint;
	readonly Action<IApplicationBuilder>                        _configure;

	public ApplyBlazorWebApplication(Assembly[] assemblies, Action<IApplicationBuilder> configure)
		: this(assemblies.AsConfiguration(), configure) {}

	public ApplyBlazorWebApplication(ICommand<RazorComponentsEndpointConventionBuilder> endpoints,
	                                 Action<IApplicationBuilder> configure)
		: this("__EndpointRouteBuilder", endpoints, configure) {}

	public ApplyBlazorWebApplication(string name, ICommand<RazorComponentsEndpointConventionBuilder> endpoint,
	                                 Action<IApplicationBuilder> configure)
	{
		_name      = name;
		_endpoint  = endpoint;
		_configure = configure;
	}

	public void Execute(IApplicationBuilder parameter)
	{
		var builder = parameter.Properties[_name]
		                       .Verify()
		                       .To<IEndpointRouteBuilder>()
		                       .MapRazorComponents<T>()
		                       .AddInteractiveServerRenderMode();
		_endpoint.Execute(builder);
		_configure(parameter);
	}
}