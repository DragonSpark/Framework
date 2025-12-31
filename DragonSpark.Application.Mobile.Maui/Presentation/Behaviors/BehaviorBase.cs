using System;
using CommunityToolkit.Maui.Behaviors;
using Microsoft.Maui.Controls;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Behaviors;

public class BehaviorBase : BehaviorBase<VisualElement>;

public class BehaviorBase<T> : BaseBehavior<T> where T : VisualElement
{
    protected sealed override void OnAttachedTo(T bindable)
    {
        bindable.Unloaded              += OnUnloaded;
        bindable.BindingContextChanged += Bindable_BindingContextChanged;
        BindingContext                 =  bindable.BindingContext;
        base.OnAttachedTo(bindable);
        OnAttached(bindable);
    }

    void FireDetachedFrom(T bindable)
    {
        OnDetachingFrom(bindable);
    }

    void OnUnloaded(object? sender, EventArgs e)
    {
        if (sender is T view)
        {
            FireDetachedFrom(view);
        }
    }

    protected virtual void OnAttached(T bindable) {}
    protected virtual void OnDetached(T bindable) {}

    void Bindable_BindingContextChanged(object? sender, EventArgs e)
    {
        if (sender is BindableObject b)
        {
            OnBindingContextChanged(b);
        }
    }

    protected virtual void OnBindingContextChanged(BindableObject b)
    {
        BindingContext = b.BindingContext;
    }

    protected sealed override void OnDetachingFrom(T bindable)
    {
        bindable.Unloaded              -= OnUnloaded;
        bindable.BindingContextChanged -= Bindable_BindingContextChanged;
        OnDetached(bindable);
        base.OnDetachingFrom(bindable);
    }
}