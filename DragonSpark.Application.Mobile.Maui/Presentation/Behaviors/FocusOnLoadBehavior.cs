using System;
using Microsoft.Maui.Controls;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Behaviors;

public sealed class FocusOnLoadBehavior : BehaviorBase<VisualElement>
{
    public readonly static BindableProperty TargetElementProperty =
        BindableProperty.Create(nameof(TargetElement), typeof(VisualElement), typeof(FocusOnLoadBehavior));

    public VisualElement? TargetElement
    {
        get => (VisualElement?)GetValue(TargetElementProperty);
        set => SetValue(TargetElementProperty, value);
    }
    
    protected override void OnAttachedTo(VisualElement bindable)
    {
        base.OnAttachedTo(bindable);

        bindable.Loaded += OnChanged;    
    }

    protected override void OnDetachingFrom(VisualElement bindable)
    {
        bindable.Loaded -= OnChanged;
        base.OnDetachingFrom(bindable);
    }

    void OnChanged(object? sender, EventArgs e)
    {
        TargetElement?.Focus();
    }
}