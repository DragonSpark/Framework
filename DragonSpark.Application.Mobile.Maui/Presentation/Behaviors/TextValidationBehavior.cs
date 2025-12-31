using DragonSpark.Compose;
using Microsoft.Maui.Controls;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Behaviors;

public sealed class TextValidationBehavior : CommunityToolkit.Maui.Behaviors.TextValidationBehavior
{
    AttachmentMonitor? _monitor;

    protected override void OnAttachedTo(BindableObject bindable)
    {
        _monitor?.Dispose();
        _monitor = new AttachmentMonitor(this, (VisualElement)bindable, _ => {}, OnDetachingFrom);
        _monitor.Execute();
        base.OnAttachedTo(bindable);
    }

    protected override void OnDetachingFrom(BindableObject bindable)
    {
        _monitor?.Dispose();
        _monitor = null;
        base.OnDetachingFrom(bindable);
    }
}