using System;

namespace DragonSpark.Application.Mobile.Maui.Configuration;

public sealed record RemoteConfigurationSettings
{
    public required Guid Identity { get; set; }
}