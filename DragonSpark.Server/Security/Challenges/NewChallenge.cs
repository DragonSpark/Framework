using DragonSpark.Contracts.Security;
using DragonSpark.Model.Selection;
using DragonSpark.Text;

namespace DragonSpark.Server.Security.Challenges;

sealed class NewChallenge : INewChallenge
{
    readonly ISelect<string, ChallengeTokenPayload> _payload;
    readonly IFormatter<ChallengeTokenPayload>      _format;

    public NewChallenge(PayloadFormatter formatter) : this(ConstructPayload.Default, formatter) {}

    public NewChallenge(ISelect<string, ChallengeTokenPayload> payload, IFormatter<ChallengeTokenPayload> format)
    {
        _payload = payload;
        _format  = format;
    }

    public ChallengeResponse Get(string parameter)
    {
        var payload = _payload.Get(parameter);
        var token   = _format.Get(payload);
        return new(payload.Challenge, token);
    }
}