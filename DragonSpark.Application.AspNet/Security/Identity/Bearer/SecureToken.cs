namespace DragonSpark.Application.AspNet.Security.Identity.Bearer;

sealed class SecureToken : TokenBase, ISecureToken
{
    public SecureToken(EncryptedClaimsSecurityDescriptor descriptor) : base(descriptor) {}
}