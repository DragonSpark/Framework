using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Reflection.Selection;
using DragonSpark.Runtime.Environment;
using System;
using System.Reflection;

namespace DragonSpark.Composition;

sealed class CreateModularity : ISelect<string, Modularity>
{
	public static CreateModularity Default { get; } = new();

	CreateModularity() : this(TypeSelection<PublicAssemblyTypes>.Default.Get) {}

	readonly Func<Array<Type>, IComponentTypes> _locator;
	readonly Func<string, Array<Assembly>>      _select;
	readonly Func<Array<Assembly>, Array<Type>> _types;

	public CreateModularity(Func<Array<Assembly>, Array<Type>> types)
		: this(ComponentTypeLocators.Default.Get, types, EnvironmentAwareAssemblies.Default.Get) {}

	public CreateModularity(Func<Array<Type>, IComponentTypes> locator, Func<Array<Assembly>, Array<Type>> types,
	                        Func<string, Array<Assembly>> select)
	{
		_locator = locator;
		_types   = types;
		_select  = select;
	}

	public Modularity Get(string parameter)
	{
		var assemblies = _select(parameter);
		var types      = _types(assemblies);
		var locator    = _locator(types);
		return new(assemblies, types, locator, new ComponentType(locator));
	}
}