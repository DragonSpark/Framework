using DragonSpark.Model.Selection;
using Polly;
using System.Net.Http;

namespace DragonSpark.Application.Communication
{
	public interface ICommunicationsPolicy
		: ISelect<PolicyBuilder<HttpResponseMessage>, IAsyncPolicy<HttpResponseMessage>> {}
}