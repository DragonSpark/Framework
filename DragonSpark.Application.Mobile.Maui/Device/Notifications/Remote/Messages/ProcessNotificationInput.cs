namespace DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote.Messages;

public readonly record struct ProcessNotificationInput(string Title, string Body, string? Action);