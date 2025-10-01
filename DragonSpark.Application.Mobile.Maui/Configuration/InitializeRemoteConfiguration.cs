using System;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Runtime.Initialization;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Hosting;

namespace DragonSpark.Application.Mobile.Maui.Configuration;

sealed class InitializeRemoteConfiguration : IMauiInitializeService
{
    public static InitializeRemoteConfiguration Default { get; } = new();

    InitializeRemoteConfiguration() : this(RegisterInitialization.Default) {}

    readonly ICommand<Task> _register;

    public InitializeRemoteConfiguration(ICommand<Task> register) => _register = register;

    public void Initialize(IServiceProvider services)
    {
        _register.Execute(services.GetRequiredService<SaveIdentity>().Allocate(CancellationToken.None));
    }
}