using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DeviceCheck;
using DragonSpark.Application.Mobile.Attestation;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Results.Stop;
using Foundation;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Attestation;

sealed class AttestationToken : IAttestationToken
{
    public static AttestationToken Default { get; } = new();

    public AttestationToken() : this(DCAppAttestService.SharedService, ClientKey.Default) {}

    readonly DCAppAttestService _service;
    readonly IStopAware<string> _key;

    public AttestationToken(DCAppAttestService service, IStopAware<string> key)
    {
        _service = service;
        _key     = key;
    }

    public async ValueTask<string> Get(Stop<string> parameter)
    {
        if (!_service.Supported)
        {
            throw new NotSupportedException("App Attest not supported on this device.");
        }

        await ClearClientKey.Default.Get(new(None.Default, parameter)); // TODO
        var bytes       = Convert.FromBase64String(parameter);
        var hash        = SHA256.HashData(bytes);
        var data        = NSData.FromArray(hash);
        var key         = await _key.Off(parameter);
        var attestation = await _service.AttestKeyAsync(key, data).Off();
        return Convert.ToBase64String(attestation.ToArray());
    }
}