using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace DragonSpark.Application.Hosting.Server.Blazor;

sealed class ApplyBlazorWebApplication<T> : ICommand<IApplicationBuilder>
{
	public static ApplyBlazorWebApplication<T> Default { get; } = new();

	ApplyBlazorWebApplication() : this("__EndpointRouteBuilder") {}

	readonly string _name;

	public ApplyBlazorWebApplication(string name) => _name = name;

	public void Execute(IApplicationBuilder parameter)
	{
		parameter.Properties[_name]
		         .Verify()
		         .To<IEndpointRouteBuilder>()
		         .MapRazorComponents<T>()
		         .AddInteractiveServerRenderMode();
	}
}