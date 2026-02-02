using System.Security;
using System.Security.Cryptography;
using DragonSpark.Application.Runtime.Objects;
using DragonSpark.Contracts.Security;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime;
using DragonSpark.Text;

namespace DragonSpark.Server.Security.Challenges;

sealed class ComposePayload : ISelect<ComposePayloadInput, ChallengeTokenPayload?>
{
    public static ComposePayload Default { get; } = new();

    ComposePayload()
        : this(DefaultSerializer<ChallengeTokenPayload>.Default.Parser, EncodedTextAsData.Default, Time.Default) {}

    readonly IParser<ChallengeTokenPayload> _parser;
    readonly IParser<byte[]>                _bytes;
    readonly ITime                          _time;

    public ComposePayload(IParser<ChallengeTokenPayload> parser, IParser<byte[]> bytes, ITime time)
    {
        _parser = parser;
        _bytes  = bytes;
        _time   = time;
    }

    public ChallengeTokenPayload? Get(ComposePayloadInput parameter)
    {
        var (contents, signature, expected) = parameter;
        if (CryptographicOperations.FixedTimeEquals(_bytes.Get(signature), _bytes.Get(expected)))
        {
            var payload = _parser.Get(contents);
            var now     = _time.Get().ToUnixTimeSeconds();
            return now > payload.ExpiresAt ? throw new SecurityException("Token expired") : payload;
        }

        return null;
    }
}