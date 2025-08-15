using System.Net.Http;
using System.Threading.Tasks;
using DragonSpark.Application.Communication.Http.Security;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Communication.Http;

sealed class DefaultAccessTokenProvider : IAccessTokenProvider
{
    public static DefaultAccessTokenProvider Default { get; } = new();

    public DefaultAccessTokenProvider() : this(ProcessBearer.Default) {}

    readonly IResult<string?> _bearer;

    public DefaultAccessTokenProvider(IResult<string?> bearer) => _bearer = bearer;

    public ValueTask<string?> Get(Stop<HttpRequestMessage> parameter) => _bearer.Get().ToOperation();
}