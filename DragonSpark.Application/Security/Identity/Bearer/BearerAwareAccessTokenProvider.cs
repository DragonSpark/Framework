using DragonSpark.Application.Communication.Http;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;
using System.Net.Http;

namespace DragonSpark.Application.Security.Identity.Bearer;

sealed class BearerAwareAccessTokenProvider : Maybe<Stop<HttpRequestMessage>, string>, IAccessTokenProvider
{
	public BearerAwareAccessTokenProvider(IAccessTokenProvider previous, CurrentMessageBearer current)
		: base(previous, current) {}
}