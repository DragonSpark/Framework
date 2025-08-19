using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Attestation;
using DragonSpark.Application.Runtime.Objects;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Attestation;

sealed class KeyAwareAttestationToken : IAttestationToken
{
    readonly IAttestationToken     _previous;
    readonly IStorageValue<string> _key;

    public KeyAwareAttestationToken(IAttestationToken previous) : this(previous, ClientKeyStorageValue.Default) {}

    public KeyAwareAttestationToken(IAttestationToken previous, IStorageValue<string> key)
    {
        _previous = previous;
        _key      = key;
    }

    public async ValueTask<string> Get(Stop<string> parameter) => await _key.Get(parameter.Token).Off() is not null
                                                                      ? await _previous.Off(parameter)
                                                                      : string.Empty;
}