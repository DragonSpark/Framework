using System;
using DragonSpark.Text;
using Windows.System.Profile;

namespace DragonSpark.Application.Mobile.Run;

public sealed class CurrentPlatformMoniker : IText
{
    public static CurrentPlatformMoniker Default { get; } = new();

    CurrentPlatformMoniker() : this(AnalyticsInfo.VersionInfo) {}

    readonly AnalyticsVersionInfo _version;

    public CurrentPlatformMoniker(AnalyticsVersionInfo version) => _version = version;

    public string Get()
    {
        var family  = _version.DeviceFamily.AsSpan();
        var content = family[..family.IndexOf('.')];
        var moniker = content is "Win32NT" ? "Windows" : content;
        return new(moniker);
    }
}