using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Behaviors;

public sealed class FocusOnVisibleBehavior : ExecuteOnVisibleBehaviorBase
{
    protected override void Execute()
    {
        TargetElement?.Focus();
    }
}

public sealed class ExecuteOnVisibleBehavior : ExecuteOnVisibleBehaviorBase
{
    public readonly static BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ExecuteOnVisibleBehavior));

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }
    
    public readonly static BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ExecuteOnVisibleBehavior));

    public object? CommandParameter
    {
        get => (object?)GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    protected override void Execute()
    {
        Command.Execute(CommandParameter);
    }
}

// TODO

public abstract class ExecuteOnVisibleBehaviorBase : BehaviorBase<VisualElement>
{
    public readonly static BindableProperty TargetElementProperty =
        BindableProperty.Create(nameof(TargetElement), typeof(VisualElement), typeof(FocusOnVisibleBehavior));

    public VisualElement? TargetElement
    {
        get => (VisualElement?)GetValue(TargetElementProperty);
        set => SetValue(TargetElementProperty, value);
    }
    
    protected override void OnAttachedTo(VisualElement bindable)
    {
        base.OnAttachedTo(bindable);

        bindable.PropertyChanged += OnChanged;    
    }

    protected override void OnDetachingFrom(VisualElement bindable)
    {
        bindable.PropertyChanged -= OnChanged;
        base.OnDetachingFrom(bindable);
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