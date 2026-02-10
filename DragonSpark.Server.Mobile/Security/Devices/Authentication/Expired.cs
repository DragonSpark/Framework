using System;
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.Extensions.Options;

namespace DragonSpark.Server.Mobile.Security.Devices.Authentication;

sealed class Expired : ICondition<long>
{
    readonly IOptionsMonitor<DevicePoPOptions> _options;

    public Expired(IOptionsMonitor<DevicePoPOptions> options) => _options = options;

    public bool Get(long parameter)
    {
        var options = _options.CurrentValue;
        var now     = options.TimeProvider.Verify().GetUtcNow().ToUnixTimeSeconds();
        return Math.Abs(now - parameter) > options.MaxSkew.TotalSeconds;
    }
}