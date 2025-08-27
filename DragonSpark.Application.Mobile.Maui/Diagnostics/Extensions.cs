using DragonSpark.Application.Mobile.Maui.Run;
using DragonSpark.Model.Selection;
using Microsoft.Maui.Hosting;

namespace DragonSpark.Application.Mobile.Maui.Diagnostics;

public static class Extensions
{
    public static IRunApplication WithInitialization(this ISelect<MauiAppBuilder, MauiApp> @this)
        => new InitializationAwareRunApplication(@this);
}