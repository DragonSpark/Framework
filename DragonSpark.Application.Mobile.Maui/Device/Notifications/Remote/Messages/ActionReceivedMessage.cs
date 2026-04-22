using System.Windows.Input;

namespace DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote.Messages;

public sealed record ActionReceivedMessage(string Title, string Body, ICommand Action)
    : NotificationReceivedMessage(Title, Body);