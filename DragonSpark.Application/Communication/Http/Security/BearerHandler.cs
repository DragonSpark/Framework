using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Compose;

namespace DragonSpark.Application.Communication.Http.Security;

public sealed class BearerHandler : DelegatingHandler
{
	readonly IAccessTokenProvider _provider;
	
	public BearerHandler(IAccessTokenProvider provider) => _provider = provider;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                 CancellationToken cancellationToken)
	{
		var token = await _provider.Off(new(request, cancellationToken));
		if (token is not null)
		{
			request.Headers.Authorization = new("Bearer", token);
		}

		return await base.SendAsync(request, cancellationToken).Off();
	}
}