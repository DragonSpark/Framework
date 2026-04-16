using System;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Runtime.Initialization;

sealed class InitializationServices : Variable<IServiceProvider>, IServiceProvider
{
    public static InitializationServices Default { get; } = new();

    InitializationServices() {}

    public object? GetService(Type serviceType) => Get()?.GetService(serviceType);
}