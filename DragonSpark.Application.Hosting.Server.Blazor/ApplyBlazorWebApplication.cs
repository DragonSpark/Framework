using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System.Reflection;

namespace DragonSpark.Application.Hosting.Server.Blazor;

sealed class ApplyBlazorWebApplication<T> : ICommand<IApplicationBuilder>
{
	readonly string          _name;
	readonly Array<Assembly> _assemblies;

	public ApplyBlazorWebApplication(Array<Assembly> assemblies) : this("__EndpointRouteBuilder", assemblies) {}

	public ApplyBlazorWebApplication(string name, Array<Assembly> assemblies)
	{
		_name       = name;
		_assemblies = assemblies;
	}

	public void Execute(IApplicationBuilder parameter)
	{
		parameter.Properties[_name]
		         .Verify()
		         .To<IEndpointRouteBuilder>()
		         .MapRazorComponents<T>()
		         .AddInteractiveServerRenderMode()
		         .AddAdditionalAssemblies(_assemblies);
	}
}