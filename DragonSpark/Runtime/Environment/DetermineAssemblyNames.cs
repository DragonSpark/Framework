using System.Reflection;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Runtime.Environment;

sealed class DetermineAssemblyNames : IArray<ModularityInput, IAlteration<AssemblyName>>
{
    public static DetermineAssemblyNames Default { get; } = new();

    DetermineAssemblyNames() {}

    public Array<IAlteration<AssemblyName>> Get(ModularityInput parameter)
    {
        var (platform, environment) = parameter;
        return platform is not null
                   ? new IAlteration<AssemblyName>[]
                   {
                       new SpecificPlatformEnvironmentAssemblyName(platform, environment),
                       new PlatformEnvironmentAssemblyName(platform),
                       new SpecificEnvironmentAssemblyName(environment), EnvironmentAssemblyName.Default
                   }
                   : [new SpecificEnvironmentAssemblyName(environment), EnvironmentAssemblyName.Default];
    }
}