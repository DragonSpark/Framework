using System;
using System.Security.Cryptography;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Security.Data;

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