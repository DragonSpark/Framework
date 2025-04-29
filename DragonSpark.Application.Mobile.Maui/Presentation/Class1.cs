using System;
using DragonSpark.Compose;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Maui.Presentation;
internal class Class1
{
}

public abstract class ApplicationBase : Microsoft.Maui.Controls.Application, IApplication // TODO
{
    protected ApplicationBase(IServiceProvider services) => Services = services;

    public IServiceProvider Services { get; }
}

public sealed class CurrentServices : Result<IServiceProvider>, IServiceProvider
{
    public static CurrentServices Default { get; } = new();

    CurrentServices() : base(() => Microsoft.Maui.Controls.Application.Current.Verify().To<IApplication>().Services) {}

    public object? GetService(Type serviceType) => Get().GetService(serviceType);
}


public interface IApplication
{
    IServiceProvider Services { get; }
}