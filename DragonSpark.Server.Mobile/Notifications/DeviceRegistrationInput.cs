using Microsoft.Azure.NotificationHubs;

namespace DragonSpark.Server.Mobile.Notifications;

public sealed record DeviceRegistrationInput(string DeviceIdentifier, string Channel, NotificationPlatform Platform);