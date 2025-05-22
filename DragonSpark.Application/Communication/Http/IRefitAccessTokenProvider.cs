using System.Net.Http;
using DragonSpark.Model.Operations.Allocated;

namespace DragonSpark.Application.Communication.Http;

public interface IRefitAccessTokenProvider : IAllocatedStopAware<HttpRequestMessage, string>;