using DragonSpark.Model.Operations.Allocated.Stop;
using System.Net.Http;

namespace DragonSpark.Application.Communication.Http;

public interface IAccessTokenProvider : IAllocated<HttpRequestMessage, string>;