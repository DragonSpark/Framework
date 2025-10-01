using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Attestation;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.Mobile.Maui.Configuration;

sealed class SaveIdentity : IStopAware
{
    readonly RemoteConfigurationSettings                                                      _settings;
    readonly ClientHash                                                                       _hash;
    readonly DragonSpark.Model.Operations.Results.Stop.IStopAware<ExistingAttestationResult?> _identity;

    public SaveIdentity(RemoteConfigurationSettings settings, ClientHash hash)
        : this(settings, hash, AttestationIdentity.Default) {}

    public SaveIdentity(RemoteConfigurationSettings settings, ClientHash hash,
                        DragonSpark.Model.Operations.Results.Stop.IStopAware<ExistingAttestationResult?> identity)
    {
        _settings = settings;
        _hash     = hash;
        _identity = identity;
    }

    public async ValueTask Get(CancellationToken parameter)
    {
        var hash = await _hash.Off(parameter);
        await _identity.Off(new ExistingAttestationResult(_settings.Identity, hash).Stop(parameter));
    }
}