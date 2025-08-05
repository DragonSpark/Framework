using System.Collections.Generic;
using DragonSpark.Model.Sequences;
using Microsoft.Extensions.Configuration;

namespace DragonSpark.Application.AspNet.Configuration;

sealed class HostedConfiguration : IHostedConfiguration
{
    readonly IConfiguration _configuration;
    readonly Array<string>  _keys;

    public HostedConfiguration(IConfiguration configuration, HostedConfigurationSettings settings)
        : this(configuration, settings.Keys) {}

    public HostedConfiguration(IConfiguration configuration, Array<string> keys)
    {
        _configuration = configuration;
        _keys          = keys;
    }

    public IReadOnlyDictionary<string, object?> Get()
    {
        var result = new Dictionary<string, object?>();
        foreach (var key in _keys)
        {
            var value = _configuration[key];
            result.Add(key, value);
        }

        return result;
    }
}