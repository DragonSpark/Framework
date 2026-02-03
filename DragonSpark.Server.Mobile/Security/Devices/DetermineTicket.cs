using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Server.Mobile.Security.Devices;

sealed class DetermineTicket : IStopAware<DetermineTicketInput, AuthenticateResult>
{
    readonly OptionsAwareEmitNonce            _emit;
    readonly ValidatePayload                  _payload;
    readonly ISelect<HttpRequest, JwsResult?> _parser;

    public DetermineTicket(OptionsAwareEmitNonce emit, ValidatePayload payload)
        : this(emit, payload, JwsHeaderParser.Default) {}

    public DetermineTicket(OptionsAwareEmitNonce emit, ValidatePayload payload, ISelect<HttpRequest, JwsResult?> parser)
    {
        _emit    = emit;
        _payload = payload;
        _parser  = parser;
    }

    public async ValueTask<AuthenticateResult> Get(Stop<DetermineTicketInput> parameter)
    {
        var ((subject, record, scheme), stop) = parameter;
        await _emit.Off(new(subject, stop));

        var parsed = _parser.Get(subject.Request);
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