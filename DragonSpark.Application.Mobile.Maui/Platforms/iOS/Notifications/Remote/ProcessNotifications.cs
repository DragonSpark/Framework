using DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote.Messages;
using DragonSpark.Compose;
using Foundation;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Notifications.Remote;

sealed class ProcessNotifications : IProcessNotifications
{
    readonly IProcessNotification _process;
    readonly string               _action;

    public ProcessNotifications(IProcessNotification process) : this(process, ActionKey.Default) {}

    public ProcessNotifications(IProcessNotification process, string action)
    {
        _process = process;
        _action  = action;
    }

    public void Execute(NSDictionary parameter)
    {
        var (title, body, action) = Extract(parameter);
        _process.Execute(new(title ?? "Money Clouds Notification", body.EmptyIfNull(), action));
    }

    (string? Title, string? Body, string? Action) Extract(NSDictionary userInfo)
        => userInfo.ObjectForKey(new NSString("aps")) is NSDictionary aps
               ? aps.ObjectForKey(new NSString("alert")) switch
               {
                   NSDictionary x => (x.ObjectForKey(new NSString("title")) as NSString,
                                      x.ObjectForKey(new NSString("body")) as NSString, x[_action] as NSString),
                   NSString simpleAlert => (null, simpleAlert, null),
                   _ => (null, null, null)
               } : (null, null, null);
}