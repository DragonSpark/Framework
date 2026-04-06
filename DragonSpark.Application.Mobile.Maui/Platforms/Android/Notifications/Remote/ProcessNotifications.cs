using Android.Content;
using DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote;
using DragonSpark.Application.Mobile.Maui.Messaging;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Notifications.Remote;

sealed class ProcessNotifications : IProcessNotifications
{
    public static ProcessNotifications Default { get; } = new();

    ProcessNotifications() : this(ActionKey.Default, Send<ActionReceivedMessage>.Default) {}

    readonly string                          _key;
    readonly ICommand<ActionReceivedMessage> _send;

    public ProcessNotifications(string key, ICommand<ActionReceivedMessage> send)
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
                _send.Execute(new(action));
            }
        }
    }
}