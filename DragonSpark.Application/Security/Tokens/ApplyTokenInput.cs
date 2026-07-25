namespace DragonSpark.Application.Security.Tokens;

public readonly record struct ApplyTokenInput(HttpRequestMessage Request, HttpResponseMessage Response);