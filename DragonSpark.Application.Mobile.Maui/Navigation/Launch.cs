using System;
using DragonSpark.Application.Mobile.Maui.Model.Navigation;
using Microsoft.Maui.ApplicationModel;

namespace DragonSpark.Application.Mobile.Maui.Navigation;

public sealed class Launch : AsynchronousCommand<Uri>
{
    public static Launch Default { get; } = new();

    Launch() : base(Launcher.OpenAsync!) {}
}