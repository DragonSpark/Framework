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

    protected override void OnAttached(VisualElement bindable)
    {
        base.OnAttached(bindable);

        bindable.Loaded += OnChanged;
    }

    protected override void OnDetached(VisualElement bindable)
    {
        bindable.Loaded -= OnChanged;
        base.OnDetached(bindable);
    }

    void OnChanged(object? sender, EventArgs e)
    {
        var element = TargetElement ?? View;
        element?.Focus();
    }
}