using System.Net.Http;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.Security.Tokens;

sealed class ApplyProof : IStopAware<HttpRequestMessage>
{
    readonly CreateProof _proof;
    readonly ITokens     _tokens;
    readonly string      _name;

    public ApplyProof(CreateProof proof, ITokens tokens) : this(proof, tokens, ProofName.Default) {}

    public ApplyProof(CreateProof proof, ITokens tokens, string name)
    {
        _proof  = proof;
        _tokens = tokens;
        _name   = name;
    }

    public async ValueTask Get(Stop<HttpRequestMessage> parameter)
    {
        var (subject, stop) = parameter;
        var origin = Origins.Default.Get(subject);
        var nonce  = _tokens.Get(origin);
        var proof  = await _proof.Off(new(new(subject, nonce), stop));

        subject.Headers.Remove(_name);
        subject.Headers.TryAddWithoutValidation(_name, proof);
    }
}