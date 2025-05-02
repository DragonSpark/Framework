using CommunityToolkit.Maui.Core;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Components.Notification;

public readonly record struct ToastInput(
    string Message,
    double FontSize = 14,
    ToastDuration Duration = ToastDuration.Short);