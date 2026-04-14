using DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote;
using DragonSpark.Application.Mobile.Maui.Messaging;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Foundation;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Notifications.Remote;

sealed class ProcessNotifications : IProcessNotifications
{
    public static ProcessNotifications Default { get; } = new();

    ProcessNotifications() : this(Send<AlertReceivedMessage>.Default, new(ActionKey.Default)) {}

    readonly ICommand<AlertReceivedMessage> _send;
    readonly NSString                       _action;

    public ProcessNotifications(ICommand<AlertReceivedMessage> send, NSString action)
    {
        _send   = send;
        _action = action;
    }

    public void Execute(NSDictionary parameter)
    {
        var (title, body) = ExtractAlert(parameter);
        _send.Execute(new(title ?? "Money Clouds Notification", body.EmptyIfNull(), 
                          parameter.ObjectForKey(_action) is NSString action ? action.Description : string.Empty ));
    }

    (string? Title, string? Body) ExtractAlert(NSDictionary userInfo)
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