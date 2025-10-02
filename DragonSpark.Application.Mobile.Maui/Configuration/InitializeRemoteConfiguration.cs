using System;
using DragonSpark.Application.Mobile.Runtime.Initialization;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Hosting;

namespace DragonSpark.Application.Mobile.Maui.Configuration;

sealed class InitializeRemoteConfiguration : IMauiInitializeService
{
    public static InitializeRemoteConfiguration Default { get; } = new();

    InitializeRemoteConfiguration() : this(RegisterInitialization.Default) {}

    readonly ICommand<IOperation> _register;

    public InitializeRemoteConfiguration(ICommand<IOperation> register) => _register = register;

    public void Initialize(IServiceProvider services)
    {
        _register.Execute(services.GetRequiredService<SaveIdentity>());
    }
}