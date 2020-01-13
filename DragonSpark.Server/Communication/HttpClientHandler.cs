using DragonSpark.Compose;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Server.Communication
{
	class HttpClientHandler : System.Net.Http.HttpClientHandler
	{
		readonly ILogger _logger;

		public HttpClientHandler(ILogger<HttpClientHandler> logger) => _logger = logger;

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
		                                                       CancellationToken cancellationToken)
		{
			_logger.LogDebug("Resource request made for {Uri}.", request.RequestUri);
			return base.SendAsync(request, cancellationToken);
		}
	}
}