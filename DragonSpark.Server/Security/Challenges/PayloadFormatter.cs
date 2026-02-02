using DragonSpark.Application.AspNet.Navigation;
using DragonSpark.Application.Runtime.Objects;
using DragonSpark.Contracts.Security;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Text;

namespace DragonSpark.Server.Security.Challenges;

sealed class PayloadFormatter : IFormatter<ChallengeTokenPayload>
{
    public PayloadFormatter(IChallengeHasher hasher)
        : this(DefaultSerializer<ChallengeTokenPayload>.Default, Base64UrlEncode.Default, hasher) {}

    readonly ISerializer<ChallengeTokenPayload> _serializer;
    readonly IAlteration<string>                _encode, _hash;

    public PayloadFormatter(ISerializer<ChallengeTokenPayload> serializer, IAlteration<string> encode,
                            IAlteration<string> hash)
    {
        _serializer = serializer;
        _encode     = encode;
        _hash       = hash;
    }

    public string Get(ChallengeTokenPayload parameter)
    {
        var content = _serializer.Get(parameter);
        return $"{_encode.Get(content)}.{_hash.Get(content)}";
    }
}