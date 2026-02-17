using DragonSpark.Model.Selection;
using DragonSpark.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace DragonSpark.Application.Security.Tokens;

public sealed class Base64UrlData : Select<string, byte[]>, IParser<byte[]>
{
    public static Base64UrlData Default { get; } = new();

    Base64UrlData() : base(WebEncoders.Base64UrlDecode) {}
}