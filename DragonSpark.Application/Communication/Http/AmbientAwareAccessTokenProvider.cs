using System.Net.Http;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Communication.Http;

sealed class AmbientAwareAccessTokenProvider : IAccessTokenProvider
{
    public static AmbientAwareAccessTokenProvider Default { get; } = new();

    AmbientAwareAccessTokenProvider() : this(AmbientBearer.Default) {}

    readonly IResult<string?> _bearer;

    public AmbientAwareAccessTokenProvider(IResult<string?> bearer) => _bearer = bearer;

    public ValueTask<string?> Get(Stop<HttpRequestMessage> parameter) => _bearer.Get().ToOperation();
}