using System;
using CommunityToolkit.Maui.Behaviors;
using Microsoft.Maui.Controls;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Behaviors;

public class BehaviorBase<T> : BaseBehavior<T> where T : VisualElement
{
    protected override void OnAttachedTo(T bindable)
    {
        bindable.BindingContextChanged += Bindable_BindingContextChanged;
        BindingContext                 =  bindable.BindingContext;
        base.OnAttachedTo(bindable);
    }

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

    protected override void OnDetachingFrom(T bindable)
    {
        bindable.BindingContextChanged -= Bindable_BindingContextChanged;
        base.OnDetachingFrom(bindable);
    }
}