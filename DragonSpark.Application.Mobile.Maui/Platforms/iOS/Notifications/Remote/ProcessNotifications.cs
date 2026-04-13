using DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote;
using DragonSpark.Application.Mobile.Maui.Messaging;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Foundation;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Notifications.Remote;

sealed class ProcessNotifications : IProcessNotifications
{
    public static ProcessNotifications Default { get; } = new();

    ProcessNotifications() : this(Send<ActionReceivedMessage>.Default, new(ActionKey.Default)) {}

    readonly ICommand<ActionReceivedMessage> _send;
    readonly NSString                        _action;

    public ProcessNotifications(ICommand<ActionReceivedMessage> send, NSString action)
    {
        _send   = send;
        _action = action;
    }

    public void Execute(NSDictionary parameter)
    {
        if (parameter.ObjectForKey(_action) is NSString action && !action.Description.IsNullOrWhiteSpace())
        {
            _send.Execute(new(action.Description));
        }
    }
}