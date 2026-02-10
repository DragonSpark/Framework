using DragonSpark.Contracts.Security;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime;
using DragonSpark.Text;

namespace DragonSpark.Server.Security.Challenges;

sealed class ConstructPayload : ISelect<string, ChallengeTokenPayload>
{
    public static ConstructPayload Default { get; } = new();

    ConstructPayload() : this(Challenges.Default, Time.Default) {}

    readonly IText _challenge;
    readonly ITime _time;

    public ConstructPayload(IText challenge, ITime time)
    {
        _challenge = challenge;
        _time      = time;
    }

    public ChallengeTokenPayload Get(string parameter)
    {
        var time      = _time.Get();
        var challenge = _challenge.Get();
        return new(challenge, time.ToUnixTimeSeconds(), time.AddMinutes(2).ToUnixTimeSeconds(), parameter);
    }
}