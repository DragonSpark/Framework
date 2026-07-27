using DragonSpark.Application.Mobile.Runtime.Initialization;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.Mobile.Maui.Configuration;

sealed class InitializeRemoteConfiguration : IMauiInitializeService
{
    public static InitializeRemoteConfiguration Default { get; } = new();

    InitializeRemoteConfiguration() : this(RegisterInitialization.Default) {}

    readonly ICommand<IStopAware> _register;

    public InitializeRemoteConfiguration(ICommand<IStopAware> register) => _register = register;

    public void Initialize(IServiceProvider services)
    {
        _register.Execute(services.GetRequiredService<SaveIdentity>());
    }
}