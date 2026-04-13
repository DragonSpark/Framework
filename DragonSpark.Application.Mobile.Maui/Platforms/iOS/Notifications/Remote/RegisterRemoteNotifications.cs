using DragonSpark.Model.Commands;
using UIKit;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Notifications.Remote;

sealed class RegisterRemoteNotifications : Command
{
    public static RegisterRemoteNotifications Default { get; } = new();

    RegisterRemoteNotifications() : this(UIApplication.SharedApplication) {}

    public RegisterRemoteNotifications(UIApplication application) : base(application.RegisterForRemoteNotifications) {}
}