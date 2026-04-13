using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using UserNotifications;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Notifications.Remote;

sealed class Register : ICommand
{
    public static Register Default { get; } = new();

    Register()
        : this(UNUserNotificationCenter.Current,
               UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound,
               RegisterRemoteNotifications.Default.AsMainThreadAware()) {}

    readonly UNUserNotificationCenter _center;
    readonly UNAuthorizationOptions   _options;
    readonly ICommand<None>           _register;

    public Register(UNUserNotificationCenter center, UNAuthorizationOptions options, ICommand<None> register)
    {
        _center   = center;
        _options  = options;
        _register = register;
    }

    public void Execute(None parameter)
    {
        _center.RequestAuthorization(_options, (granted, error) =>
                                               {
                                                   if (granted && error.Account() == null)
                                                   {
                                                       _register.Execute();
                                                   }
                                               });
    }
}