using System.Reflection;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Runtime.Environment;

sealed class EnvironmentAwareAssemblies : IArray<ModularityInput, Assembly>
{
    public static EnvironmentAwareAssemblies Default { get; } = new();

    EnvironmentAwareAssemblies() {}

    public Array<Assembly> Get(ModularityInput parameter)
    {
        var names    = new ComponentAssemblyNames(parameter);
        var selector = new AssemblySelector(names);
        var result   = new Assemblies(selector).Get();
        return result;
    }
}