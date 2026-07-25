using DragonSpark.Compose;

namespace DragonSpark.Application.Security.Tokens;

sealed class DevicePoPHandler : DelegatingHandler
{
    readonly IDeviceKeyProvider _keys;
    readonly ApplyProof         _proof;
    readonly ProcessResponse    _response;

    public DevicePoPHandler(IDeviceKeyProvider keys, ApplyProof proof, ProcessResponse response)
    {
        _keys     = keys;
        _proof    = proof;
        _response = response;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        var deviceId = (await _keys.Off(ct)).Jkt;
        request.Headers.Authorization = new(SchemeName.Default, deviceId);

        await _proof.Off(new(request, ct));

        // Send & handle 401 nonce challenge (retry once)
        var first = await base.SendAsync(request, ct).Off();
        var next  = await _response.Off(new(first, ct));
        if (next is not null)
        {
            first.Dispose();
            var result = await base.SendAsync(next, ct).Off();
            await _response.Off(new(result, ct));
            return result;
        }

        return first;
    }
}