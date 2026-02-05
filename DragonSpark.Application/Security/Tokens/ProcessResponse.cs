using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.Security.Tokens;

sealed class ProcessResponse : IStopAware<HttpResponseMessage, HttpRequestMessage?>
{
    readonly ITokens      _tokens;
    readonly CloneRequest _clone;
    readonly string       _name;

    public ProcessResponse(ITokens tokens, CloneRequest clone) : this(tokens, clone, TokenName.Default) {}

    public ProcessResponse(ITokens tokens, CloneRequest clone, string name)
    {
        _tokens = tokens;
        _clone  = clone;
        _name   = name;
    }

    public async ValueTask<HttpRequestMessage?> Get(Stop<HttpResponseMessage> parameter)
    {
        var (subject, stop) = parameter;
        if (subject.Headers.TryGetValues(_name, out var values))
        {
            var value = values.FirstOrDefault();
            if (!value.IsNullOrEmpty() && subject.RequestMessage is not null)
            {
                _tokens.Execute((subject.RequestMessage.RequestUri.Verify(), value));
                switch (subject.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        return await _clone.Off(new(subject.RequestMessage, stop));
                }
            }
        }

        return null;
    }
}