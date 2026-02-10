using System;
using System.Security.Cryptography;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Security.Tokens;

sealed class Tokens : ISelect<byte, string>
{
    public static Tokens Default { get; } = new();

    Tokens() {}

    public string Get(byte parameter)
    {
        Span<byte> bytes = stackalloc byte[parameter];
        RandomNumberGenerator.Fill(bytes);
        return Convert.ToBase64String(bytes);
    }
}