using System.Net.Http;

namespace DragonSpark.Application.Security.Tokens;

public readonly record struct ApplyTokenInput(HttpRequestMessage Request, HttpResponseMessage Response, string Token);