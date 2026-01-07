using DragonSpark.Compose;
using Microsoft.Maui.Controls;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Behaviors;

public sealed class EventToCommandBehavior : CommunityToolkit.Maui.Behaviors.EventToCommandBehavior
{
    AttachmentMonitor? _monitor;

    protected override void OnAttachedTo(VisualElement bindable)
    {
        _monitor?.Dispose();
        _monitor = new AttachmentMonitor(this, bindable);
        _monitor.Execute();
        base.OnAttachedTo(bindable);
    }

    protected override void OnDetachingFrom(VisualElement bindable)
    {
        _monitor?.Dispose();
        _monitor = null;
        base.OnDetachingFrom(bindable);
    }

}
