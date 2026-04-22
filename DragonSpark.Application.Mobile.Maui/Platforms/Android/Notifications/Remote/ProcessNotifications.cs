using Android.Content;
using DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote.Messages;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Notifications.Remote;

sealed class ProcessNotifications : IProcessNotifications
{
    readonly string               _key;
    readonly IProcessNotification _process;

    public ProcessNotifications(IProcessNotification process) : this("notification", process) {}

    public ProcessNotifications(string key, IProcessNotification process)
    {
        _key     = key;
        _process = process;
    }

    public void Execute(Intent parameter)
    {
        if (parameter.HasExtra(_key))
        {
            var title = parameter.GetStringExtra("title") ?? parameter.GetStringExtra("notification.title")
                        ?? "Money Clouds Notification";
            var body = parameter.GetStringExtra("body")
                       ?? parameter.GetStringExtra("notification.body")
                       ?? parameter.GetStringExtra("message")
                       ?? string.Empty;
            _process.Execute(new(title, body, parameter.GetStringExtra(ActionKey.Default)));
        }
    }
}