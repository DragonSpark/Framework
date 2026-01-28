using System;
using System.Text.Json;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using Microsoft.AspNetCore.Authentication;

namespace DragonSpark.Server.Mobile.Security.Devices;

sealed class ValidatePayload : IStopAware<ValidatePayloadInput, AuthenticateResult?>
{
    readonly ValidatePayloadBody _body;
    readonly StringComparison    _comparison;

    public ValidatePayload(ValidatePayloadBody body, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
    {
        _body       = body;
        _comparison = comparison;
    }

    public async ValueTask<AuthenticateResult?> Get(Stop<ValidatePayloadInput> parameter)
    {
        var ((request, payload), stop) = parameter;
        using var document = JsonDocument.Parse(payload);
        var       root     = document.RootElement;
        return root.TryGetProperty("htm", out var htm) && root.TryGetProperty("htu", out var htu) &&
               root.TryGetProperty("iat", out var iatEl)
                   ? long.TryParse(iatEl.ToString(), out var iat)
                         ? string.Equals(htm.GetString(), request.Method, _comparison)
                               ? await _body.Off(new(new(request, root, htu.GetString(), iat), stop))
                               : AuthenticateResult.Fail("htm mismatch")
                         : AuthenticateResult.Fail("Invalid iat")
                   : AuthenticateResult.Fail("Invalid DPoP payload");
    }
}