using Microsoft.Maui.Controls;

namespace DragonSpark.Application.Mobile.Maui.Presentation;

public readonly record struct PushModalInput(Page Subject, bool Animated = true);