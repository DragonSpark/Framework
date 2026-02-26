using System;
using System.Threading.Tasks;
using DeviceCheck;
using DragonSpark.Application.Mobile.Attestation;
using DragonSpark.Application.Security.Tokens;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Results.Stop;
using DragonSpark.Text;
using Foundation;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Attestation.Assertion;

sealed class AssertionToken : IAssertionToken
{
    public static AssertionToken Default { get; } = new();

    AssertionToken() : this(DCAppAttestService.SharedService, HashedBase64UrlData.Default, ClientKey.Default) {}

    readonly DCAppAttestService _service;
    readonly IParser<byte[]>    _hash;
    readonly IStopAware<string> _key;

    public AssertionToken(DCAppAttestService service, IParser<byte[]> hash, IStopAware<string> key)
    {
        _service = service;
        _hash    = hash;
        _key     = key;
    }

    public async ValueTask<string> Get(Stop<string> parameter)
    {
        if (_service.Supported)
        {
            var hash      = _hash.Get(parameter);
            var data      = NSData.FromArray(hash);
            var key       = await _key.Off(parameter);
            var assertion = await _service.GenerateAssertionAsync(key, data).Off();
            return Convert.ToBase64String(assertion.ToArray());
        }

        return string.Empty;
    }
}