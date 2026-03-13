using System;
using System.Windows.Input;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;

namespace DragonSpark.Application.Mobile.Maui.Device.Input;

public sealed class AdjustInputCommand : ICommand
{
    public static AdjustInputCommand Default { get; } = new();

    AdjustInputCommand() {}

    public event EventHandler? CanExecuteChanged = delegate{};

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter)
    {
        Microsoft.Maui.Controls.Application.Current?.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>()
                 .UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
    }
}