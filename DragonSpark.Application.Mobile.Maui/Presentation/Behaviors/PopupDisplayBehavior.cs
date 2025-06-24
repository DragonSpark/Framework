using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;
using Syncfusion.Maui.Toolkit.Popup;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Behaviors;

public sealed class PopupDisplayBehavior : BehaviorBase<SfPopup>
{
    public static readonly BindableProperty MarginProperty
        = BindableProperty.Create(nameof(Margin), typeof(Thickness), typeof(PopupDisplayBehavior));

    public Thickness Margin
    {
        get { return (Thickness)GetValue(MarginProperty); }
        set { SetValue(MarginProperty, value); }
    }

    protected override void OnAttachedTo(SfPopup bindable)
    {
        var display = DeviceDisplay.MainDisplayInfo;
        var density = display.Density;
        var height  = display.Height / density;
        var width   = display.Width / density;
        var popupX  = (width - bindable.WidthRequest) / 2;
        var popupY  = height - bindable.HeightRequest - Margin.Bottom;
        bindable.Show(popupX, popupY);
        base.OnAttachedTo(bindable);
    }
}