using System;
using DragonSpark.Model.Results;
using Windows.System.Profile;

namespace DragonSpark.Application.Mobile.Run;

public sealed class GetPlatformVersion : IResult<PlatformVersion>
{
    public static GetPlatformVersion Default { get; } = new();

    GetPlatformVersion() : this(AnalyticsInfo.VersionInfo) {}

    readonly AnalyticsVersionInfo _version;

    public GetPlatformVersion(AnalyticsVersionInfo version) => _version = version;

    public PlatformVersion Get()
    {
        var span = _version.DeviceFamily.AsSpan();
        var info = span[..span.IndexOf('.')];
        return new(new(info), _version.DeviceFamilyVersion);
    }
}