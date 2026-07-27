using DragonSpark.Compose;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Behaviors;

public sealed class TextValidationBehavior : CommunityToolkit.Maui.Behaviors.TextValidationBehavior
{
    AttachmentMonitor? _monitor;
    /*
    protected override void OnAttachedTo(BindableObject bindable)
    {
        _monitor?.Dispose();
        _monitor = new AttachmentMonitor(this, (VisualElement)bindable, OnDetachingFrom);
        _monitor.Execute();
        base.OnAttachedTo(bindable);
    }

    protected override void OnDetachingFrom(BindableObject bindable)
    {
        _monitor?.Dispose();
        _monitor = null;
        base.OnDetachingFrom(bindable);
    }*/

    protected override void OnAttachedTo(VisualElement bindable)
    {
        base.OnAttachedTo(bindable);
        _monitor?.Dispose();
        _monitor = new AttachmentMonitor(this, bindable, OnDetachingFrom);
        _monitor.Execute();
    }

    protected override void OnDetachingFrom(VisualElement bindable)
    {
        _monitor?.Dispose();
        _monitor = null;
        base.OnDetachingFrom(bindable);
    }
    /*protected override void OnAttachedTo(BindableObject bindable)
    {
        BindingContext                 =  bindable.BindingContext;
        bindable.BindingContextChanged += Bindable_BindingContextChanged;
        base.OnAttachedTo(bindable);
    }

    void Bindable_BindingContextChanged(object? sender, EventArgs e)
    {
        if (sender is BindableObject b)
        {
            BindingContext = b.BindingContext;
        }
    }

    protected override void OnDetachingFrom(BindableObject bindable)
    {
        bindable.BindingContextChanged -= Bindable_BindingContextChanged;
        base.OnDetachingFrom(bindable);
    }*/
}