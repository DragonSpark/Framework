using System;
using DragonSpark.Text;

namespace DragonSpark.Application.Security.Data;

public sealed class NonceFormatter : IFormatter<string>
{
    public static NonceFormatter Default { get; } = new();

    NonceFormatter() {}

    public string Get(string parameter)
    {
        var source = parameter.AsSpan().TrimEnd('=');
        var result = source.Length <= 256 ? stackalloc char[source.Length] : new char[source.Length];
        source.Replace(result, '+', '-');
        result.Replace('/', '_');
        return new(result);
    }
}