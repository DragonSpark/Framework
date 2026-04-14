using Android.Content;
using DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote;
using DragonSpark.Application.Mobile.Maui.Messaging;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Notifications.Remote;

sealed class ProcessNotifications : IProcessNotifications
{
    public static ProcessNotifications Default { get; } = new();

    ProcessNotifications() : this(ActionKey.Default, Send<AlertReceivedMessage>.Default) {}

    readonly string                         _key;
    readonly ICommand<AlertReceivedMessage> _send;

    public ProcessNotifications(string key, ICommand<AlertReceivedMessage> send)
    {
        _key  = key;
        _send = send;
    }

    public void Execute(Intent parameter)
    {
        if (parameter.HasExtra(_key))
        {
            var action = parameter.GetStringExtra(_key);
            if (!action.IsNullOrEmpty())
            {
                var title = parameter.GetStringExtra("title") ?? parameter.GetStringExtra("notification.title")
                            ?? "Money Clouds Notification";
                var body = parameter.GetStringExtra("body")
                           ?? parameter.GetStringExtra("notification.body")
                           ?? parameter.GetStringExtra("message")
                           ?? string.Empty;
                _send.Execute(new(title, body, action));
            }
        }
    }
}