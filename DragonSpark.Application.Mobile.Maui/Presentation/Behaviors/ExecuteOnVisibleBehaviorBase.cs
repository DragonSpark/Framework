using System.ComponentModel;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Behaviors;

public abstract class ExecuteOnVisibleBehaviorBase : BehaviorBase<VisualElement>
{
    public readonly static BindableProperty TargetElementProperty =
        BindableProperty.Create(nameof(TargetElement), typeof(VisualElement), typeof(FocusOnVisibleBehavior));

    public VisualElement? TargetElement
    {
        get => (VisualElement?)GetValue(TargetElementProperty);
        set => SetValue(TargetElementProperty, value);
    }
    
    protected override void OnAttached(VisualElement bindable)
    {
        base.OnAttached(bindable);

        bindable.PropertyChanged += OnChanged;    
    }

    protected override void OnDetached(VisualElement bindable)
    {
        bindable.PropertyChanged -= OnChanged;
        base.OnDetached(bindable);
    }

    protected abstract void Execute();
    void OnChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(View.IsVisible) && (View?.IsVisible ?? false))
        {
            Execute();
        }
    }
}