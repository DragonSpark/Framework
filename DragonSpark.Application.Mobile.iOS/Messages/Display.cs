using System.Threading.Tasks;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Results;
using UIKit;

namespace DragonSpark.Application.Mobile.iOS.Messages;

sealed class Display : IAllocated<DisplayInput>
{
    public static Display Default { get; } = new();

    Display() : this(CurrentController.Default) {}

    readonly IResult<UIViewController?> _controller;

    public Display(IResult<UIViewController?> controller) => _controller = controller;

    public Task Get(DisplayInput parameter)
    {
        var (title, message) = parameter;
        var source = new TaskCompletionSource<bool>();
        var alert  = UIAlertController.Create(title, message, UIAlertControllerStyle.Alert);
        alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, _ => source.SetResult(true)));
        _controller.Get()?.PresentViewController(alert, true, null);
        return source.Task;
    }
}