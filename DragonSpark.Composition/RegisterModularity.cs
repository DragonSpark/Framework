using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;

namespace DragonSpark.Composition;

sealed class RegisterModularity : IServiceConfiguration
{
	public static RegisterModularity Default { get; } = new();

	RegisterModularity() : this(ModularityComponents.Default) {}

	readonly ISelect<IHostEnvironment, Modularity> _components;

	public RegisterModularity(ISelect<IHostEnvironment, Modularity> components) => _components = components;

	public void Execute(IServiceCollection parameter)
	{
		var (assemblies, types, locator, componentType) =
			_components.Get(parameter.GetRequiredInstance<IHostEnvironment>());

		parameter.AddSingleton<IArray<Assembly>>(new Instances<Assembly>(assemblies))
		         .AddSingleton<IArray<Type>>(new Instances<Type>(types))
		         .AddSingleton(locator)
		         .AddSingleton(componentType);
	}
}