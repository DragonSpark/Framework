using System;
using Microsoft.Maui.Controls;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Behaviors;

public sealed class TextValidationBehavior : CommunityToolkit.Maui.Behaviors.TextValidationBehavior
{
    protected override void OnAttachedTo(BindableObject bindable)
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
    }
}