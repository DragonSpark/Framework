namespace DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote.Messages;

public sealed record AlertReceivedMessage(string Title, string Body) : NotificationReceivedMessage(Title, Body);