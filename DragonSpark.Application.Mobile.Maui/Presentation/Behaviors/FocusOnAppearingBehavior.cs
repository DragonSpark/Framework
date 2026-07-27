using DragonSpark.Compose;

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

    protected override void OnAttached(Page bindable)
    {
        base.OnAttached(bindable);

        bindable.Appearing += OnPageAppearing;
    }

    protected override void OnDetached(Page bindable)
    {
        bindable.Appearing -= OnPageAppearing;
        base.OnDetached(bindable);
    }

    void OnPageAppearing(object? sender, EventArgs e)
    {
        if (TargetElement is not null)
        {
            // Small delay to ensure layout is complete (MAUI quirk)
            MainThread.InvokeOnMainThreadAsync(async () =>
                                               {
                                                   await Task.Delay(100).On(); // 100ms is usually enough
                                                   TargetElement.Focus();
                                               });
        }
    }
}