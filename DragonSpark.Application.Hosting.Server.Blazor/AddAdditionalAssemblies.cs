using DragonSpark.Model.Commands;
using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.Builder;
using System.Reflection;

namespace DragonSpark.Application.Hosting.Server.Blazor;

sealed class AddAdditionalAssemblies : ICommand<RazorComponentsEndpointConventionBuilder>
{
	readonly Array<Assembly> _assemblies;

	public AddAdditionalAssemblies(Array<Assembly> assemblies) => _assemblies = assemblies;

	public void Execute(RazorComponentsEndpointConventionBuilder parameter)
	{
		parameter.AddAdditionalAssemblies(_assemblies);
	}
}