using System.Net.Http;
using DragonSpark.Model.Selection;
using Polly;

namespace DragonSpark.Application.Communication;

public interface ICommunicationsPolicy : ISelect<PolicyBuilder<HttpResponseMessage>, IAsyncPolicy<HttpResponseMessage>>;
