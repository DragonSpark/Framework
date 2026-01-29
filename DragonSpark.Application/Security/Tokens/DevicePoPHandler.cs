using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Compose;

namespace DragonSpark.Application.Security.Tokens;

sealed class DevicePoPHandler : DelegatingHandler
{
    readonly IDeviceKeyProvider _keys;
    readonly CreateProof        _proof;
    readonly ITokens            _tokens;

    public DevicePoPHandler(IDeviceKeyProvider keys, CreateProof proof, ITokens tokens)
    {
        _keys   = keys;
        _proof  = proof;
        _tokens = tokens;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        // Authorization: DevicePoP <deviceId=jkt>
        var deviceId = (await _keys.Off(ct)).Jkt;
        request.Headers.Authorization = new("DevicePoP", deviceId);

        // DPoP proof (with last known nonce if any)
        var origin = new Uri(request.RequestUri!.GetLeftPart(UriPartial.Authority));
        var nonce  = _tokens.Get(origin);
        var proof  = await _proof.Off(new(new(request, nonce), ct));

        request.Headers.Remove("DPoP");
        request.Headers.TryAddWithoutValidation("DPoP", proof);

        // Send & handle 401 nonce challenge (retry once)
        var resp = await base.SendAsync(request, ct).Off();
        if ((int)resp.StatusCode == 401 && resp.Headers.TryGetValues("DPoP-Nonce", out var vals))
        {
            var newNonce = vals.FirstOrDefault();
            if (!string.IsNullOrEmpty(newNonce))
            {
                _tokens.Execute((origin, newNonce));

                resp.Dispose();

                var clone  = await CloneMessage.Default.Off(new(request, ct));
                var proof2 = await _proof.Off(new(new(clone, newNonce), ct));

                clone.Headers.Remove("DPoP");
                clone.Headers.TryAddWithoutValidation("DPoP", proof2);

                return await base.SendAsync(clone, ct).Off();
            }
        }

        // Cache next nonce if server supplies it on success
        if (resp.Headers.TryGetValues("DPoP-Nonce", out var next))
        {
            var n = next.FirstOrDefault();
            if (!n.IsNullOrEmpty())
            {
                _tokens.Execute((origin, n));
            }
        }

        return resp;
    }
}