using System;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Attestation;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.Mobile.Maui.Configuration;

sealed class SaveIdentity : IOperation
{
    readonly Func<RemoteConfigurationSettings>     _settings;
    readonly ClientHash                            _hash;
    readonly IStopAware<ExistingAttestationResult> _set;

    public SaveIdentity(Func<RemoteConfigurationSettings> settings, ClientHash hash)
        : this(settings, hash, SaveAttestationIdentity.Default) {}

    public SaveIdentity(Func<RemoteConfigurationSettings> settings, ClientHash hash,
                        IStopAware<ExistingAttestationResult> set)
    {
        _settings = settings;
        _hash     = hash;
        _set      = set;
    }

    public async ValueTask Get()
    {
        var hash     = await _hash.Off(CancellationToken.None);
        var identity = _settings().Identity;
        await _set.Off(new(new(identity, hash), CancellationToken.None));
    }
}