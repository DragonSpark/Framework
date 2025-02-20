using System;
using System.Reflection;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Reflection.Selection;
using DragonSpark.Runtime.Environment;

namespace DragonSpark.Composition;

sealed class CreateModularity : ISelect<ModularityInput, Modularity>
{
    public static CreateModularity Default { get; } = new();

    CreateModularity() : this(TypeSelection<PublicAssemblyTypes>.Default.Get) {}

    readonly Func<Array<Type>, IComponentTypes>     _locator;
    readonly Func<ModularityInput, Array<Assembly>> _assemblies;
    readonly Func<Array<Assembly>, Array<Type>>     _types;

    public CreateModularity(Func<Array<Assembly>, Array<Type>> types)
        : this(EnvironmentAwareAssemblies.Default.Get, types, ComponentTypeLocators.Default.Get) {}

    public CreateModularity(Func<ModularityInput, Array<Assembly>> assemblies, Func<Array<Assembly>, Array<Type>> types,
                            Func<Array<Type>, IComponentTypes> locator)
    {
        _assemblies = assemblies;
        _types      = types;
        _locator    = locator;
    }

    public Modularity Get(ModularityInput parameter)
    {
        var assemblies = _assemblies(parameter);
        var types      = _types(assemblies);
        return new(assemblies, types, _locator(types));
    }
}