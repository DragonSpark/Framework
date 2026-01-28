using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Security.Tokens;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Text;
using Microsoft.AspNetCore.Authentication;

namespace DragonSpark.Server.Mobile.Security.Devices;

sealed class DetermineTicket : IStopAware<DetermineTicketInput, AuthenticateResult>
{
    readonly OptionsAwareApplyNonce _apply;
    readonly ValidatePayload        _payload;
    readonly IParser<JwsResult?>    _parser;

    public DetermineTicket(OptionsAwareApplyNonce apply, ValidatePayload payload)
        : this(apply, payload, JwsParser.Default) {}

    public DetermineTicket(OptionsAwareApplyNonce apply, ValidatePayload payload, IParser<JwsResult?> parser)
    {
        _apply   = apply;
        _payload = payload;
        _parser  = parser;
    }

    public async ValueTask<AuthenticateResult> Get(Stop<DetermineTicketInput> parameter)
    {
        var ((subject, record, scheme), stop) = parameter;
        await _apply.Off(new(new(subject, NoncePurpose.Other), stop));

        var header = subject.Request.Headers["DPoP"].ToString();
        var parsed = _parser.Get(header);
        if (parsed is not null)
        {
            using var result = parsed.Value;
            var (hdrJson, payload, signingInput, sigRaw) = result;
            return ValidateHeader.Default.Get(hdrJson.AsMemory())
                   ?? ValidateHash.Default.Get(new(record, signingInput, sigRaw.AsMemory()))
                   ?? await _payload.Off(new(new(subject.Request, payload.AsMemory()), stop))
                   ?? SuccessfulTicket.Default.Get(new(record.DeviceId, scheme));
        }

        return AuthenticateResult.Fail("Invalid or missing DPoP JWS");
    }
}