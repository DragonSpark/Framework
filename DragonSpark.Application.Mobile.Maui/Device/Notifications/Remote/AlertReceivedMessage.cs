namespace DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote;

public sealed record AlertReceivedMessage(string Title, string Body, string Action);