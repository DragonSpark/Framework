using System.Threading.Tasks;
using CommunityToolkit.Maui.Alerts;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Components.Notification;

sealed class DisplayToast : IStopAware<ToastInput>
{
    public static DisplayToast Default { get; } = new();

    DisplayToast() {}

    public ValueTask Get(Stop<ToastInput> parameter)
    {
        var ((message, fontSize, duration), stop) = parameter;
        var toast  = Toast.Make(message, duration, fontSize);
        return toast.Show(stop).ToOperation();
    }
}