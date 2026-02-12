using System.Net;

namespace DragonSpark.Application.AspNet.Communication;

public sealed class AuthorizationHeaderName : HeaderName
{
    public static AuthorizationHeaderName Default { get; } = new();

    AuthorizationHeaderName() : base(HttpRequestHeader.Authorization) {}
}