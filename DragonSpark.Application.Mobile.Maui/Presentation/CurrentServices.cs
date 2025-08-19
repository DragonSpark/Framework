using System;
using DragonSpark.Compose;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Maui.Presentation;

public sealed class CurrentServices : Result<IServiceProvider>, IServiceProvider
{
    public static CurrentServices Default { get; } = new();

    CurrentServices() : base(() => CurrentSystemServiceProvider.Default.Get().Verify()) {}

    public object? GetService(Type serviceType) => Get().GetService(serviceType);
}