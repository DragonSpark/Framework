using System;
using System.Linq;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Devices;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Markup;

public sealed class UriMarkupExtension : MarkupExtension<Uri>
{
    public required string Address { get; set; }

    public override Uri ProvideValue(IServiceProvider serviceProvider) => new(Address);
}

public enum MauiPlatform : byte
{
    iOS,
    Android,
    WinUI,
    MacCatalyst,
    Tizen,
    Unknown
}

public sealed class IsPlatformExtension : IMarkupExtension<bool>
{
    public static IsPlatformExtension iOS { get; } = new() { Platform = MauiPlatform.iOS };

    public MauiPlatform Platform { get; set; } = MauiPlatform.Unknown;

    public string Platforms { get; set; } = string.Empty; // comma-separated

    public bool ProvideValue(IServiceProvider serviceProvider)
    {
        var current = DeviceInfo.Platform.ToString() switch
        {
            nameof(DevicePlatform.iOS) => MauiPlatform.iOS,
            nameof(DevicePlatform.Android) => MauiPlatform.Android,
            nameof(DevicePlatform.WinUI) => MauiPlatform.WinUI,
            nameof(DevicePlatform.MacCatalyst) => MauiPlatform.MacCatalyst,
            nameof(DevicePlatform.Tizen) => MauiPlatform.Tizen,
            _ => MauiPlatform.Unknown
        };

        return !string.IsNullOrWhiteSpace(Platforms)
                   ? Platforms.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                              .Select(p => Enum.TryParse<MauiPlatform>(p, ignoreCase: true, out var ep)
                                               ? ep
                                               : MauiPlatform.Unknown)
                              .Where(p => p != MauiPlatform.Unknown)
                              .Contains(current)
                   : current == Platform;
    }

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
}