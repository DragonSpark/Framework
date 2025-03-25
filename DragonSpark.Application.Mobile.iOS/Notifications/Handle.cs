using System;
using DragonSpark.Model.Commands;
using Foundation;

namespace DragonSpark.Application.Mobile.iOS.Notifications;

sealed class Handle : ICommand<NSError?>
{
    public static Handle Default { get; } = new();

    Handle() {}

    public void Execute(NSError? parameter)
    {
        if (parameter is not null)
        {
            throw new InvalidOperationException($"Failed to schedule notification: {parameter}");
        }
    }
}