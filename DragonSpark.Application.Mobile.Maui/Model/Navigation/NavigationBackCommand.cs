using DragonSpark.Application.Mobile.Maui.Model.Commands;
using DragonSpark.Model;
using Microsoft.Maui.Controls;

namespace DragonSpark.Application.Mobile.Maui.Model.Navigation;

public sealed class NavigationBackCommand : AsynchronousCommand<None>
{
    public static NavigationBackCommand Default { get; } = new();

    NavigationBackCommand() : this(Shell.Current) {}

    public NavigationBackCommand(Shell shell) : base(_ => shell.GoToAsync("..")) {}
}