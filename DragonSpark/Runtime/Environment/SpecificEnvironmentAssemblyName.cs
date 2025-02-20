using System.Reflection;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Runtime.Environment;

sealed class SpecificEnvironmentAssemblyName : IAlteration<AssemblyName>
{
    readonly string _format;
    readonly string _name;

    public SpecificEnvironmentAssemblyName(string name) : this("{0}.Environment.{1}", name) {}

    public SpecificEnvironmentAssemblyName(string format, string name)
    {
        _format = format;
        _name   = name;
    }

    public AssemblyName Get(AssemblyName parameter) => new(string.Format(_format, parameter.Name, _name));
}

sealed class SpecificPlatformEnvironmentAssemblyName : IAlteration<AssemblyName>
{
    readonly string _format, _platform, _name;

    public SpecificPlatformEnvironmentAssemblyName(string platform, string name)
        : this("{0}.Environment.{1}.{2}", platform, name) {}

    public SpecificPlatformEnvironmentAssemblyName(string format, string platform, string name)
    {
        _format   = format;
        _platform = platform;
        _name     = name;
    }

    public AssemblyName Get(AssemblyName parameter) => new(string.Format(_format, parameter.Name, _platform, _name));
}