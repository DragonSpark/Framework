using Microsoft.IdentityModel.JsonWebTokens;

namespace DragonSpark.Application.AspNet.Security.Identity.Bearer;

sealed class Token : TokenBase, IToken
{
    public Token(ClaimsSecurityDescriptor descriptor) : this(descriptor, WebTokenHandler.Default) {}

    public Token(ClaimsSecurityDescriptor descriptor, JsonWebTokenHandler handler) : base(descriptor, handler) {}
}