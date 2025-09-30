using System;
using DragonSpark.Application.Mobile.Runtime.Initialization;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Hosting;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Syncfusion;

sealed class ConfigureApplication : IMauiInitializeService
{
    public static ConfigureApplication Default { get; } = new();

    ConfigureApplication() : this(RegisterInitialization.Default) {}

    readonly ICommand<Action> _register;

    public ConfigureApplication(ICommand<Action> register) => _register = register;

    public void Initialize(IServiceProvider services)
    {
        _register.Execute(new Register(services).Then());
    }

    sealed class Register : SelectedCommand<None>, ICommand
    {
        public Register(IServiceProvider services)
            : base(Start.A.Result(services.GetRequiredService<RegisterLicense>).Accept()) {}
    }
}