namespace DragonSpark.Application.Security;

public sealed class Base64UrlNonce : NonceBase
{
    public static Base64UrlNonce Default { get; } = new();

    Base64UrlNonce()
        : base(x =>
               {
                   using var source = Tokens.Base64UrlCharacterEncoder.Default.Get(x.AsMemory());
                   return new(source.AsSpan());
               }) {}
}