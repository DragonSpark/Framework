using System;
using DragonSpark.Compose;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Maui.Presentation;

public sealed class CurrentSystemServiceProvider : Result<IServiceProvider?>
{
    public static CurrentSystemServiceProvider Default { get; } = new();

    CurrentSystemServiceProvider()
        : base(() => Microsoft.Maui.Controls.Application.Current?.To<IApplication>().Services) {}
}