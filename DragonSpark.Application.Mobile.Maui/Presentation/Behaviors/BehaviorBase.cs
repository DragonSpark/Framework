using CommunityToolkit.Maui.Behaviors;
using DragonSpark.Compose;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Behaviors;

public class BehaviorBase : BehaviorBase<VisualElement>;

public class BehaviorBase<T> : BaseBehavior<T> where T : VisualElement
{
    AttachmentMonitor? _monitor;

    protected sealed override void OnAttachedTo(T bindable)
    {
        _monitor = new AttachmentMonitor(this, bindable, 
                                         new AttachmentMonitorEvents(OnBindingContextChanged, OnDetachingFrom));
        _monitor.Execute();

        base.OnAttachedTo(bindable);
        OnAttached(bindable);
    }

    protected virtual void OnAttached(T bindable) {}

    protected virtual void OnDetached(T bindable) {}

    protected virtual void OnBindingContextChanged(BindableObject b) {}

    protected sealed override void OnDetachingFrom(T bindable)
    {
        _monitor?.Dispose();
        _monitor = null;
        OnDetached(bindable);
        base.OnDetachingFrom(bindable);
        bindable.Loaded += OnLoaded;
    }

    void OnLoaded(object? sender, EventArgs e)
    {
        if (sender is T view)
        {
            view.Loaded -= OnLoaded;
            OnAttachedTo(view);    
        }
    }
}