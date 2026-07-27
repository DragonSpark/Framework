using System.Windows.Input;
using Syncfusion.Maui.Toolkit.OtpInput;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Behaviors;

public sealed class OtpCompletedBehavior : BehaviorBase<SfOtpInput>
{
    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(OtpCompletedBehavior));

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(OtpCompletedBehavior));

    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }
    
    protected override void OnAttached(SfOtpInput bindable)
    {
        base.OnAttached(bindable);
        bindable.ValueChanged += OnValueChanged;
    }

    protected override void OnDetached(SfOtpInput bindable)
    {
        bindable.ValueChanged -= OnValueChanged;
        base.OnDetached(bindable);
    }

    void OnValueChanged(object? sender, OtpInputValueChangedEventArgs e)
    {
        if (Command is not null && e.NewValue is not null && sender is SfOtpInput bindable &&
            e.NewValue.Length == (int)bindable.Length && Command.CanExecute(CommandParameter))
        {
            Command.Execute(CommandParameter ?? e.NewValue);
        }
    }
}