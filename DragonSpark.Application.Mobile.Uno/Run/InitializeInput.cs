using Microsoft.UI.Xaml;

namespace DragonSpark.Application.Mobile.Uno.Run;

public readonly record struct InitializeInput(Microsoft.UI.Xaml.Application Owner, LaunchActivatedEventArgs Arguments);