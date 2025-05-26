using System.Net.Http;
using DragonSpark.Model.Operations.Allocated.Stop;

namespace DragonSpark.Application.Communication.Http;

public interface IRefitAccessTokenProvider : IAllocated<HttpRequestMessage, string>;