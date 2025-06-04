using Microsoft.Maui.Controls;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Behaviors;

public sealed class EventToCommandBehavior : CommunityToolkit.Maui.Behaviors.EventToCommandBehavior
{
    protected override void OnAttachedTo(VisualElement bindable)
    {
        BindingContext = bindable.BindingContext;
        base.OnAttachedTo(bindable);
    }
}

public sealed class TextValidationBehavior : CommunityToolkit.Maui.Behaviors.TextValidationBehavior
{
    protected override void OnAttachedTo(BindableObject bindable)
    {
        BindingContext                 =  bindable.BindingContext;
        bindable.BindingContextChanged += Bindable_BindingContextChanged;
        base.OnAttachedTo(bindable);
    }

    void Bindable_BindingContextChanged(object? sender, System.EventArgs e)
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