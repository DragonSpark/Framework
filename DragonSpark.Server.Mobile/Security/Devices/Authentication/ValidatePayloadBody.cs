using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Security.Tokens;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace DragonSpark.Server.Mobile.Security.Devices.Authentication;

sealed class ValidatePayloadBody : IStopAware<ValidatePayloadBodyInput, AuthenticateResult?>
{
    readonly Expired                           _expired;
    readonly IMarkUsed                         _mark;
    readonly IOptionsMonitor<DevicePoPOptions> _options;

    public ValidatePayloadBody(Expired expired, IMarkUsed mark, IOptionsMonitor<DevicePoPOptions> options)
    {
        _expired = expired;
        _mark    = mark;
        _options = options;
    }

    public async ValueTask<AuthenticateResult?> Get(Stop<ValidatePayloadBodyInput> parameter)
    {
        var ((request, root, address, iat), stop) = parameter;
        return string.Equals(address, $"{request.Scheme}://{request.Host}{request.Path}", StringComparison.Ordinal)
                   ? _expired.Get(iat)
                         ? AuthenticateResult.Fail("DPoP iat too old")
                         : _options.CurrentValue.RequireNonce
                             ? root.TryGetProperty("nonce", out var nonce)
                                   ? await Mark(nonce, stop).Off()
                                   : AuthenticateResult.Fail("Nonce required")
                             : null
                   : AuthenticateResult.Fail("htu mismatch");
    }

    async ValueTask<AuthenticateResult?> Mark(JsonElement nonce, CancellationToken stop)
        => await _mark.Off(new(nonce.GetString().EmptyIfNull(), stop))
               ? null
               : AuthenticateResult.Fail("Nonce invalid/reused");
}