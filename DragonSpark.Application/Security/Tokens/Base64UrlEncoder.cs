using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Application.Security.Tokens;

public sealed class Base64UrlEncoder : Alteration<string>
{
    public static Base64UrlEncoder Default { get; } = new();

    Base64UrlEncoder() : base(Text.Base64Encode.Default.Then().Select(TokenFormatter.Default)) {}
}