using DeviceCheck;
using DragonSpark.Application.Mobile.Attestation;
using DragonSpark.Application.Security.Tokens;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Results.Stop;
using DragonSpark.Text;
using Foundation;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Attestation;

sealed class AttestationToken : IAttestationToken
{
    public static AttestationToken Default { get; } = new();

    public AttestationToken()
        : this(DCAppAttestService.SharedService, HashedBase64UrlData.Default, ClientKey.Default) {}

    readonly DCAppAttestService _service;
    readonly IParser<byte[]>    _hash;
    readonly IStopAware<string> _key;

    public AttestationToken(DCAppAttestService service, IParser<byte[]> hash, IStopAware<string> key)
    {
        _service = service;
        _hash    = hash;
        _key     = key;
    }

    public async ValueTask<string> Get(Stop<string> parameter)
    {
        if (!_service.Supported)
        {
            throw new NotSupportedException("App Attest not supported on this device.");
        }

        var hash        = _hash.Get(parameter);
        var data        = NSData.FromArray(hash);
        var key         = await _key.Off(parameter);
        var attestation = await _service.AttestKeyAsync(key, data).Off();
        return Convert.ToBase64String(attestation.ToArray());
    }
}