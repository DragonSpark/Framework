using System;
using DragonSpark.Text;

namespace DragonSpark.Application.Security.Tokens;

public sealed class TokenFormatter : IFormatter<string>
{
    public static TokenFormatter Default { get; } = new();

    TokenFormatter() {}

    public string Get(string parameter)
    {
        var source = parameter.AsSpan().TrimEnd('=');
        var result = source.Length <= 256 ? stackalloc char[source.Length] : new char[source.Length];
        source.Replace(result, '+', '-');
        result.Replace('/', '_');
        return new(result);
    }
}