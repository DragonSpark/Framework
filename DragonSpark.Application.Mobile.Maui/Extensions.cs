using DragonSpark.Application.Mobile.Maui.Run;
using DragonSpark.Composition;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Maui;

public static class Extensions
{
	public static BuildHostContext WithFrameworkConfigurations(this BuildHostContext @this)
		=> Configure.Default.Get(@this);

	public static BuildHostContext WithIdentityProfileFrameworkConfiguration(this BuildHostContext @this)
		=> @this.Configure(Security.Identity.Registrations.Default, Security.Identity.Profile.Registrations.Default);
}

sealed class Configure : IAlteration<BuildHostContext>
{
    public static Configure Default { get; } = new();

    Configure() {}

    public BuildHostContext Get(BuildHostContext parameter)
        => parameter.Configure(Application.DefaultRegistrations.Default, DefaultRegistrations.Default);
}

sealed class DefaultRegistrations : Commands<IServiceCollection>
{
    public static DefaultRegistrations Default { get; } = new();

    DefaultRegistrations() : base(Mobile.DefaultRegistrations.Default, LocalRegistrations.Default) {}
    
}

sealed class LocalRegistrations : ICommand<IServiceCollection>
{
    /*public static LocalRegistrations Default { get; } = new();

    LocalRegistrations()
        : this(x => CurrentServices.Default.GetRequiredService<IApplicationErrorHandler>().Execute(x)) {}

    readonly Action<Exception> _error;

    public LocalRegistrations(Action<Exception> error) => _error = error;*/
    public static LocalRegistrations Default { get; } = new();

    LocalRegistrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.Start<IInitializeApplication>()
                 .Forward<DefaultInitializeApplication>()
                 .Singleton()
            ;
        // Command.DefaultErrorHandler = _error;
    }
}
// TODO