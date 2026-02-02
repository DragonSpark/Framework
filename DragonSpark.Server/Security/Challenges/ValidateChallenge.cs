using System.Security;
using DragonSpark.Contracts.Security;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Text;

namespace DragonSpark.Server.Security.Challenges;

sealed class ValidateChallenge : IValidateChallenge
{
    readonly ISelect<ComposePayloadInput, ChallengeTokenPayload?> _payload;
    readonly IAlteration<string>                                  _hash;
    readonly ISelect<string, string>                              _decode;

    public ValidateChallenge(IChallengeHasher hasher) : this(ComposePayload.Default, hasher, Base64Decode.Default) {}

    public ValidateChallenge(ISelect<ComposePayloadInput, ChallengeTokenPayload?> payload,
                             IAlteration<string> hash, ISelect<string, string> decode)
    {
        _payload = payload;
        _hash    = hash;
        _decode  = decode;
    }

    public ChallengeTokenPayload Get(string token)
    {
        var parts = token.Split('.');
        switch (parts.Length)
        {
            case 2:
            {
                var payload   = _decode.Get(parts[0]);
                var signature = parts[1];
                var expected  = _hash.Get(payload);
                return _payload.Get(new(payload, signature, expected))
                       ?? throw new SecurityException("Invalid signature");
            }
            default:
                throw new SecurityException("Invalid token format");
        }
    }
}

/*sealed class ExpirationAwareValidateChallenge : IValidateChallenge
{
    readonly IValidateChallenge _previous;
    readonly ITime              _time;

    public ExpirationAwareValidateChallenge(IValidateChallenge previous) : this(previous, Time.Default) {}

    public ExpirationAwareValidateChallenge(IValidateChallenge previous, ITime time)
    {
        _previous  = previous;
        _time = time;
    }

    public ChallengeTokenPayload? Get(string parameter)
    {
        var previous = _previous.Get(parameter);
        var result = previous?.ExpiresAt >= _time.Get().ToUnixTimeSeconds() ? previous : null;
        return result;
    }
}*/
// TODO