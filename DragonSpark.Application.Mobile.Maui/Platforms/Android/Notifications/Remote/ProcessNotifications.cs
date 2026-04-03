using Android.Content;
using CommunityToolkit.Mvvm.Messaging;
using DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote;
using DragonSpark.Compose;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Notifications.Remote;

sealed class ProcessNotifications : IProcessNotifications
{
    public static ProcessNotifications Default { get; } = new();

    ProcessNotifications() : this(ActionKey.Default, WeakReferenceMessenger.Default) {}

    readonly string     _key;
    readonly IMessenger _messenger;

    public ProcessNotifications(string key, IMessenger messenger)
    {
        _key       = key;
        _messenger = messenger;
    }

    public void Execute(Intent parameter)
    {
        if (parameter.HasExtra(_key))
        {
            var action = parameter.GetStringExtra(_key);
            if (!action.IsNullOrEmpty())
            {
                _messenger.Send(new ActionReceivedMessage(action));
            }
        }
    }
}