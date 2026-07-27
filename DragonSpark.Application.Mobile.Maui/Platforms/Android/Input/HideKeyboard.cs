using Android.App;
using Android.Content;
using Android.Views.InputMethods;
using DragonSpark.Application.Mobile.Maui.Device.Input;
using DragonSpark.Compose;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Input;

sealed class HideKeyboard : IHideKeyboard
{
    public static HideKeyboard Default { get; } = new();

    HideKeyboard() : this(Platform.AppContext, Platform.CurrentActivity.Verify()) {}

    readonly Context  _context;
    readonly Activity _activity;

    public HideKeyboard(Context context, Activity activity)
    {
        _context  = context;
        _activity = activity;
    }

    public ValueTask Get(CancellationToken parameter)
    {
        if (_context.GetSystemService(Context.InputMethodService) is InputMethodManager manager)
        {
            var token = _activity.CurrentFocus?.WindowToken ?? _activity.Window?.DecorView.WindowToken;
            if (token is not null)
            {
                manager.HideSoftInputFromWindow(token, HideSoftInputFlags.None);
            }
        }

        return ValueTask.CompletedTask;
    }
}