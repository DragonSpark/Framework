namespace DragonSpark.Application.AspNet.Communication;

public sealed class AuthorizationHeader : Header
{
    public static AuthorizationHeader Default { get; } = new();

    AuthorizationHeader() : base(AuthorizationHeaderName.Default) {}
}