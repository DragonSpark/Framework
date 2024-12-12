using Microsoft.UI.Xaml;

namespace DragonSpark.Application.Mobile.Run;

public readonly record struct InitializeInput(Microsoft.UI.Xaml.Application Owner, LaunchActivatedEventArgs Arguments);