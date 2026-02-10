using DragonSpark.Application.Mobile.Maui.Presentation.Components.Notification;
using DragonSpark.Application.Mobile.Maui.Runtime;

namespace DragonSpark.Application.Mobile.Maui.Diagnostics;

sealed class MainThreadAwareDisplayToast : MainThreadAware<ToastInput>
{
    public static MainThreadAwareDisplayToast Default { get; } = new();

    MainThreadAwareDisplayToast() : base(DisplayToast.Default) {}
}