using System.Net.Http;
using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.Communication.Http;

public interface IAccessTokenProvider : IStopAware<HttpRequestMessage, string?>;