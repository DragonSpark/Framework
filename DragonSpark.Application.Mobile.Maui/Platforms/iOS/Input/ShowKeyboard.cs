using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Maui.Device.Input;
using DragonSpark.Composition;
using DragonSpark.Model.Operations;
using Microsoft.Maui.Controls;
using UIKit;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Input;

sealed class ShowKeyboard : IShowKeyboard
{
    public static ShowKeyboard Default { get; } = new();

    ShowKeyboard() : this(UIApplication.SharedApplication) {}
    
    readonly UIApplication _application;

    [Candidate(false)]
    public ShowKeyboard(UIApplication application) => _application = application;

    public ValueTask Get(Stop<VisualElement> parameter)
    {
        _application.KeyWindow?.EndEditing(false);
        return ValueTask.CompletedTask;
    }
}