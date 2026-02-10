using DragonSpark.Application.Mobile.Maui.Device;
using DragonSpark.Application.Mobile.Maui.Device.Input;
using DragonSpark.Application.Mobile.Maui.Device.Notifications;
using DragonSpark.Application.Mobile.Maui.Diagnostics;
using DragonSpark.Application.Mobile.Maui.Platforms.Android.Input;
using DragonSpark.Application.Mobile.Maui.Platforms.Android.Notifications;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android;

sealed class Registrations : ICommand<IServiceCollection>
{
    public static Registrations Default { get; } = new();

    Registrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.TryDecorate<IConfigureExceptions, ConfigureExceptions>();
        parameter.Start<INotifications>()
                 .Forward<Notifications.Notifications>()
                 .Decorate<PermissionAwareNotifications>()
                 .Singleton()
                 //
                 .Then.Start<IHideKeyboard>()
                 .Forward<HideKeyboard>()
                 .Singleton()
                 //
                 .Then.Start<IShowKeyboard>()
                 .Forward<ShowKeyboard>()
                 .Singleton()
                 //
                 .Then.Start<IIsSimulator>()
                 .Forward<IsSimulator>()
                 .Singleton()
            ;
    }
}