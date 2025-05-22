using System;
using System.Globalization;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Behaviors;

public class BindingContextChangedCommandBehavior : Behavior<VisualElement>
{
    public readonly static BindableProperty CommandProperty 
        = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(BindingContextChangedCommandBehavior));

    public static readonly BindableProperty CommandParameterProperty 
        = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(BindingContextChangedCommandBehavior));
    public static readonly BindableProperty EventArgsConverterProperty 
        = BindableProperty.Create(nameof(EventArgsConverter), typeof(IValueConverter), typeof(BindingContextChangedCommandBehavior));

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public IValueConverter? EventArgsConverter
    {
        get => (IValueConverter?)GetValue(EventArgsConverterProperty);
        set => SetValue(EventArgsConverterProperty, value);
    }

    protected override void OnAttachedTo(VisualElement bindable)
    {
        BindingContextChanged += Bindable_BindingContextChanged;
        BindingContext        =  bindable.BindingContext;
        base.OnAttachedTo(bindable);
    }

    void Bindable_BindingContextChanged(object? sender, EventArgs e)
    {
        OnTriggerHandled(sender, e);
    }

    protected override void OnDetachingFrom(VisualElement bindable)
    {
        base.OnDetachingFrom(bindable);
        bindable.BindingContextChanged -= Bindable_BindingContextChanged;
    }

    [Microsoft.Maui.Controls.Internals.Preserve(Conditional = true)]
    protected virtual void OnTriggerHandled(object? sender = null, object? eventArgs = null)
    {
        var parameter = CommandParameter
                        ?? EventArgsConverter?.Convert(eventArgs, typeof(object), null, CultureInfo.InvariantCulture);

        var command = Command;
        if (command.CanExecute(parameter))
        {
            command.Execute(parameter);
        }
    }
}