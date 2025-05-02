using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Alerts;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Components.Notification;

sealed class DisplayToast : IOperation<ToastInput>
{
    public static DisplayToast Default { get; } = new();

    DisplayToast() {}

    public ValueTask Get(ToastInput parameter)
    {
        var (message, fontSize, duration) = parameter;
        var source = new CancellationTokenSource();
        var toast  = Toast.Make(message, duration, fontSize);
        return toast.Show(source.Token).ToOperation();
    }
}