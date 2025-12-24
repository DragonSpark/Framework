using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Behaviors;

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