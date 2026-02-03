using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.Security.Tokens;

sealed class ProcessResponse : IStopAware<HttpResponseMessage, HttpRequestMessage?>
{
    readonly ApplyToken _token;
    readonly string     _name;

    public ProcessResponse(ApplyToken token) : this(token, TokenName.Default) {}

    public ProcessResponse(ApplyToken token, string name)
    {
        _token = token;
        _name  = name;
    }

    public ValueTask<HttpRequestMessage?> Get(Stop<HttpResponseMessage> parameter)
    {
        // Cache next nonce if server supplies it on success
        var (subject, stop) = parameter;
        if (subject.Headers.TryGetValues(_name, out var values))
        {
            var value = values.FirstOrDefault();
            if (!value.IsNullOrEmpty() && subject.RequestMessage is not null)
            {
                return _token.Get(new(new(subject.RequestMessage, subject, value), stop));
            }
        }

        return ValueTask.FromResult<HttpRequestMessage?>(null);
    }
}