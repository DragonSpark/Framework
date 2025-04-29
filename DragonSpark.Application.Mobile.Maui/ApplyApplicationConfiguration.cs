using System;
using System.Reflection;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Maui.Hosting;

namespace DragonSpark.Application.Mobile.Maui;

sealed class ApplyApplicationConfiguration<T> : ICommand<MauiAppBuilder>
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

    public void Execute(MauiAppBuilder parameter)
    {
        using var @base       = _assembly.GetManifestResourceStream($"{_namespace}.appsettings.json");
        var       key         = $"{_namespace}.appsettings.{parameter.Services.EnvironmentName().ToLower()}.json";
        using var environment = _assembly.GetManifestResourceStream(key);
        var       builder     = new ConfigurationBuilder();
        
        if (@base is not null)
        {
            builder.AddJsonStream(@base);
        }

        if (environment is not null)
        {
            builder.AddJsonStream(environment);
        }

        var configuration = builder.Build();
        parameter.Configuration.AddConfiguration(configuration);
    }
}