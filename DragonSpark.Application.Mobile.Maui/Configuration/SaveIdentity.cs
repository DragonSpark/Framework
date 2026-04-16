using System;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Attestation;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.Mobile.Maui.Configuration;

sealed class SaveIdentity : IStopAware
{
    readonly Func<RemoteConfigurationSettings>   _settings;
    readonly IClientKeyHash                      _hash;
    readonly IStopAware<ValidationIdentityView> _set;

    public SaveIdentity(Func<RemoteConfigurationSettings> settings, IClientKeyHash hash)
        : this(settings, hash, SaveAttestationIdentity.Default) {}

    public SaveIdentity(Func<RemoteConfigurationSettings> settings, IClientKeyHash hash,
                        IStopAware<ValidationIdentityView> set)
    {
        _settings = settings;
        _hash     = hash;
        _set      = set;
    }

    public async ValueTask Get(CancellationToken parameter)
    {
        var hash     = await _hash.Off(parameter);
        var identity = _settings().Identity;
        await _set.Off(new(new(identity, hash), parameter));
    }
}