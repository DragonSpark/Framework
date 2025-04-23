using DragonSpark.Model.Results;
using UIKit;

namespace DragonSpark.Application.Mobile.Uno.Platforms.iOS.Messages;

sealed class CurrentController : IResult<UIViewController?>
{
    public static CurrentController Default { get; } = new();

    CurrentController() {}

    public UIViewController? Get()
    {
        var window = UIApplication.SharedApplication.KeyWindow;
        var result = window?.RootViewController;

        while (result?.PresentedViewController != null)
        {
            result = result.PresentedViewController;
        }

        return result;
    }
}