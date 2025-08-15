using System;
using DragonSpark.Compose;
using DragonSpark.Composition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Configuration;

public static class Extensions
{
    public static IServiceCollection AddRemoteConfiguration(this IServiceCollection @this,
                                                            ConfigurationManager configuration)
        => configuration.AddRemoteConfiguration(@this.DeferredEnhanced<IRemoteConfigurationMessage>()).Return(@this);

    public static ConfigurationManager AddRemoteConfiguration(this ConfigurationManager @this,
                                                              Func<IRemoteConfigurationMessage> message)
    {
        @this.Sources.Add(new RemoteConfigurationSource(message));
        return @this;
    }
}