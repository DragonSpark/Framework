using DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote.Messages;
using DragonSpark.Compose;
using Foundation;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Notifications.Remote;

sealed class ProcessNotifications : IProcessNotifications
{
    readonly IProcessNotification _process;
    readonly NSString             _action;

    public ProcessNotifications(IProcessNotification process) : this(process, new(ActionKey.Default)) {}
    
    public ProcessNotifications(IProcessNotification process, NSString action)
    {
        _process = process;
        _action  = action;
    }

    public void Execute(NSDictionary parameter)
    {
        var (title, body) = Extract(parameter);
        _process.Execute(new(title ?? "Money Clouds Notification", body.EmptyIfNull(),
                             parameter.ObjectForKey(_action) is NSString action ? action.Description : null));
    }

    static (string? Title, string? Body) Extract(NSDictionary userInfo)
        => userInfo.ObjectForKey(new NSString("aps")) is not NSDictionary aps
               ? (null, null)
               : aps.ObjectForKey(new NSString("alert")) switch
               {
                   NSDictionary alertDict => (
                                                 alertDict.ObjectForKey(new NSString("title")) as NSString,
                                                 Body: alertDict.ObjectForKey(new NSString("body")) as NSString
                                             ),

                   NSString simpleAlert => (null, simpleAlert),

                   _ => (null, null)
               };
}