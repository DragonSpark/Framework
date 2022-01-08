using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Reflection.Selection;
using DragonSpark.Runtime.Environment;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;

namespace DragonSpark.Composition;

sealed class CreateModularityComponents : ISelect<IHostEnvironment, Modularity>
{
	public static CreateModularityComponents Default { get; } = new();

	CreateModularityComponents() : this(TypeSelection<PublicAssemblyTypes>.Default.Get) {}

	readonly Func<Array<Type>, IComponentTypes> _locator;
	readonly Func<string, Array<Assembly>>      _select;
	readonly Func<Array<Assembly>, Array<Type>> _types;

	public CreateModularityComponents(Func<Array<Assembly>, Array<Type>> types)
		: this(ComponentTypeLocators.Default.Get, types, EnvironmentAwareAssemblies.Default.Get) {}

	public CreateModularityComponents(Func<Array<Type>, IComponentTypes> locator,
	                                  Func<Array<Assembly>, Array<Type>> types,
	                                  Func<string, Array<Assembly>> select)
	{
		_locator = locator;
		_types   = types;
		_select  = select;
	}

	public Modularity Get(IHostEnvironment parameter)
	{
		var assemblies = _select(parameter.EnvironmentName);
		var types      = _types(assemblies);
		var locator    = _locator(types);
		return new(assemblies, types, locator, new ComponentType(locator));
	}
}