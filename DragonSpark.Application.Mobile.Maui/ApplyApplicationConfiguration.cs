using System;
using System.Reflection;
using DragonSpark.Application.Mobile.Maui.Run;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Maui;

sealed class ApplyApplicationConfiguration<T> : ICommand<IServiceCollection>
{
    public static ApplyApplicationConfiguration<T> Default { get; } = new();

    ApplyApplicationConfiguration() : this(A.Type<T>()) {}

    readonly Assembly _assembly;
    readonly string   _namespace;

    public ApplyApplicationConfiguration(Type type) : this(type.Assembly, type.Namespace.Verify()) {}

    public ApplyApplicationConfiguration(Assembly assembly, string @namespace)
    {
        _assembly  = assembly;
        _namespace = @namespace;
    }

    public void Execute(IServiceCollection parameter)
    {
        using var @base       = _assembly.GetManifestResourceStream($"{_namespace}.appsettings.json");
        var       key         = $"{_namespace}.appsettings.{parameter.EnvironmentName().ToLower()}.json";
        using var environment = _assembly.GetManifestResourceStream(key);
        var       start       = new ConfigurationBuilder();
        if (@base is not null)
        {
            start.AddJsonStream(@base);
        }

        if (environment is not null)
        {
            start.AddJsonStream(environment);
        }

        var configuration = start.Build();
        parameter.Application().Configuration.AddConfiguration(configuration);
    }
}