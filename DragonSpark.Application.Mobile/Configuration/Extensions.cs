using DragonSpark.Compose;
using DragonSpark.Composition;
using Microsoft.Extensions.Configuration;

namespace DragonSpark.Application.Mobile.Configuration;

public static class Extensions
{
    public static ConfigurationManager AddRemoteConfiguration(this ConfigurationManager @this)
        => @this.AddRemoteConfiguration(@this.Section<RemoteConfigurationSettings>().Verify().Address);

    public static ConfigurationManager AddRemoteConfiguration(this ConfigurationManager @this, string address)
    {
        @this.Sources.Add(new RemoteConfigurationSource(address));
        return @this;
    }
}