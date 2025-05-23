using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Communication.Http.Security;

public sealed class BearerHandler : DelegatingHandler
{
    readonly IAccessTokenProvider _provider;
    readonly IResult<string?>     _process;

    public BearerHandler(IAccessTokenProvider provider) : this(provider, ProcessBearer.Default) {}

    public BearerHandler(IAccessTokenProvider provider, IResult<string?> process)
    {
        _provider     = provider;
        _process = process;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                 CancellationToken cancellationToken)
    {
        var token = _process.Get() ?? await _provider.Off(new(request, cancellationToken));
        if (token is not null)
        {
            request.Headers.Authorization = new("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken).Off();
    }
}