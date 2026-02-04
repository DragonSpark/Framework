using System.Text.Json;
using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Security;
using DragonSpark.Application.Runtime.Objects;
using DragonSpark.Application.Security.Tokens;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Selection;
using DragonSpark.Text;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Server.Mobile.Security.Devices;

public interface IDeviceRegistry : IStopAware<string, DeviceRecord?>;

// TODO

sealed class ProofAwareDeviceRegistry : StopAwareMaybe<string, DeviceRecord>, IDeviceRegistry
{
    public ProofAwareDeviceRegistry(IDeviceRegistry previous, ConstructDeviceFromRequest request)
        : base(previous, request) {}
}

sealed class ConstructDeviceFromRequest : IStopAware<string, DeviceRecord?>
{
    readonly ComposeRequestJwk _jwk;
    readonly IUpsertDevice     _upsert;

    public ConstructDeviceFromRequest(ComposeRequestJwk jwk, IUpsertDevice upsert)
    {
        _jwk    = jwk;
        _upsert = upsert;
    }

    public async ValueTask<DeviceRecord?> Get(Stop<string> parameter)
    {
        var (subject, stop) = parameter;
        var jwk = _jwk.Get(subject);
        if (jwk is not null)
        {
            var (kty, crv, x, y) = jwk;
            var result = new DeviceRecord(subject, kty, crv, x, y, false, null, null, null);

            if (await _upsert.Off(new(result, stop)))
            {
                return result;
            }
        }

        return null;
    }
}

sealed class ComposeRequestJwk : IParser<JwkHeader?>
{
    readonly ICurrentContext                  _context;
    readonly ISelect<HttpRequest, JwsResult?> _parser;
    readonly IFormatter<Points>               _jkt;

    public ComposeRequestJwk(ICurrentContext context) : this(context, JwsHeaderParser.Default, ComputeJkt.Default) {}

    public ComposeRequestJwk(ICurrentContext context, ISelect<HttpRequest, JwsResult?> parser, IFormatter<Points> jkt)
    {
        _context = context;
        _parser  = parser;
        _jkt     = jkt;
    }

    public JwkHeader? Get(string parameter)
    {
        var parsed = _parser.Get(_context.Get().Request);
        if (parsed is not null)
        {
            using var instance = parsed.Value;
            var       actual   = instance.Header.AsSpan();
            var       result   = JsonSerializer.Deserialize<JwsHeader>(actual, FrameworkSerializerOptions.Default)?.Jwk;
            if (result is not null && _jkt.Get(new(result.X, result.Y)) == parameter)
            {
                return result;
            }
        }

        return null;
    }
}

sealed record JwsHeader(JwkHeader Jwk);

/*
sealed record Jwk(string Kty, string Crv, string X, string Y);*/