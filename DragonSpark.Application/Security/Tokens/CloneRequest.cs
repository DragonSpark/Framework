using System.Net.Http;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.Security.Tokens;

sealed class CloneRequest : IAltering<HttpRequestMessage>
{
    readonly IAltering<HttpRequestMessage>  _message;
    readonly IStopAware<HttpRequestMessage> _apply;

    public CloneRequest(ApplyProof apply) : this(CloneMessage.Default, apply) {}

    public CloneRequest(IAltering<HttpRequestMessage> message, IStopAware<HttpRequestMessage> apply)
    {
        _message = message;
        _apply   = apply;
    }

    public async ValueTask<HttpRequestMessage> Get(Stop<HttpRequestMessage> parameter)
    {
        var (_, stop) = parameter;
        var result = await _message.Off(parameter);
        await _apply.Off(new(result, stop));
        return result;
    }
}