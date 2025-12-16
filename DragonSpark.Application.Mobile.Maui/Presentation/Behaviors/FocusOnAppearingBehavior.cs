using System;
using System.Threading.Tasks;
using DragonSpark.Compose;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Behaviors;

public sealed class FocusOnAppearingBehavior : BehaviorBase<Page>
{
    public static readonly BindableProperty TargetElementProperty =
        BindableProperty.Create(nameof(TargetElement), typeof(VisualElement), typeof(FocusOnAppearingBehavior));

    public VisualElement? TargetElement
    {
        get => (VisualElement?)GetValue(TargetElementProperty);
        set => SetValue(TargetElementProperty, value);
    }

    
    protected override void OnAttachedTo(Page bindable)
    {
        base.OnAttachedTo(bindable);

        bindable.Appearing += OnPageAppearing;    
    }

    protected override void OnDetachingFrom(Page bindable)
    {
        bindable.Appearing -= OnPageAppearing;
        base.OnDetachingFrom(bindable);
    }

    void OnPageAppearing(object? sender, EventArgs e)
    {
        if (TargetElement is not null)
        {
            // Small delay to ensure layout is complete (MAUI quirk)
            MainThread.InvokeOnMainThreadAsync(async () =>
                                               {
                                                   await Task.Delay(100).On();  // 100ms is usually enough
                                                   TargetElement.Focus();
                                               });
        }
    }
}