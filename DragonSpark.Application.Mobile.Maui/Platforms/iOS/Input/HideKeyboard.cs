using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Maui.Device.Input;
using UIKit;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Input;

sealed class HideKeyboard : IHideKeyboard
{
    public static HideKeyboard Default { get; } = new();

    HideKeyboard() : this(UIApplication.SharedApplication) {}
    
    readonly UIApplication _application;

    public HideKeyboard(UIApplication application) => _application = application;

    public ValueTask Get(CancellationToken parameter)
    {
        _application.KeyWindow?.EndEditing(true);
        return ValueTask.CompletedTask;
    }
}