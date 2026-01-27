using System;
using System.Security.Cryptography;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
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

// TODO
public sealed class DefaultFormattedNonces : FixedSelection<byte, string>, IText
{
    public static DefaultFormattedNonces Default { get; } = new();

    DefaultFormattedNonces() : base(FormattedNonces.Default, 24) {}
}

public sealed class FormattedNonces : Select<byte, string>
{
    public static FormattedNonces Default { get; } = new();

    FormattedNonces() : base(Nonces.Default.Then().Select(NonceFormatter.Default)) {}
}

sealed class DefaultNonces : FixedSelection<byte, string>, IText
{
    public static DefaultNonces Default { get; } = new();

    DefaultNonces() : base(Nonces.Default, 24) {}
}

sealed class Nonces : ISelect<byte, string>
{
    public static Nonces Default { get; } = new();

    Nonces() {}

    public string Get(byte parameter)
    {
        Span<byte> bytes = stackalloc byte[parameter];
        RandomNumberGenerator.Fill(bytes);
        return Convert.ToBase64String(bytes);
    }
}