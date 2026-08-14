using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.Communication.Http;

public interface IRefitAccessTokenProvider : IStopAware<HttpRequestMessage, string>;