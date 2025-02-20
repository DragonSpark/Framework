using System;
using System.Reflection;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Composition;

sealed class RegisterModularity : IServiceConfiguration
{
    public static RegisterModularity Default { get; } = new();

    RegisterModularity() : this(null) {}

    readonly string?                                 _platform;
    readonly ISelect<HostBuilderContext, Modularity> _components;

    public RegisterModularity(string? platform) : this(platform, ModularityComponents.Default) {}

    public RegisterModularity(string? platform, ISelect<HostBuilderContext, Modularity> components)
    {
        _platform   = platform;
        _components = components;
    }

    public void Execute(IServiceCollection parameter)
    {
        var instance = parameter.GetRequiredInstance<HostBuilderContext>();
        new AccessPlatform(instance).Execute(_platform);

        var (assemblies, types, locator, componentType) = _components.Get(instance);

        parameter.AddSingleton<IArray<Assembly>>(new Instances<Assembly>(assemblies))
                 .AddSingleton<IArray<Type>>(new Instances<Type>(types))
                 .AddSingleton(locator)
                 .AddSingleton(componentType);
    }
}