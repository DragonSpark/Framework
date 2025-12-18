using System.Threading.Tasks;
using Android.Content;
using Android.Views;
using Android.Views.InputMethods;
using DragonSpark.Application.Mobile.Maui.Device.Input;
using DragonSpark.Model.Operations;
using Microsoft.Maui.ApplicationModel;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Input;

sealed class ShowKeyboard : IShowKeyboard
{
    public static ShowKeyboard Default { get; } = new();

    ShowKeyboard() : this(Platform.AppContext) {}

    readonly Context _context;

    public ShowKeyboard(Context context) => _context = context;

    public ValueTask Get(Stop<Microsoft.Maui.Controls.VisualElement> parameter)
    {
        if (_context.GetSystemService(Context.InputMethodService) is InputMethodManager manager)
        {
            var (subject, _) = parameter;
            if (subject.Handler?.PlatformView is View view)
            {
                manager.ShowSoftInput(view, ShowFlags.Implicit);
            }
        }

        return ValueTask.CompletedTask;
    }
}