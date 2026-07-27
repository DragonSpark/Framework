using DragonSpark.Application.Mobile.Maui.Device.Input;
using DragonSpark.Composition;
using UIKit;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Input;

sealed class HideKeyboard : IHideKeyboard
{
    public static HideKeyboard Default { get; } = new();

    HideKeyboard() : this(UIApplication.SharedApplication) {}
    
    readonly UIApplication _application;
    
    [Candidate(false)]
    public HideKeyboard(UIApplication application) => _application = application;

    public ValueTask Get(CancellationToken parameter)
    {
        _application.KeyWindow?.EndEditing(true);
        return ValueTask.CompletedTask;
    }
}