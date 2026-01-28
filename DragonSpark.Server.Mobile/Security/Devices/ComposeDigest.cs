using System;
using System.Security.Cryptography;
using DragonSpark.Model.Selection;

namespace DragonSpark.Server.Mobile.Security.Devices;

sealed class ComposeDigest : ISelect<ReadOnlyMemory<char>, ReadOnlyMemory<byte>>
{
    public static ComposeDigest Default { get; } = new();

    ComposeDigest() {}

    public ReadOnlyMemory<byte> Get(ReadOnlyMemory<char> parameter)
    {
        var        source      = parameter.Span;
        Span<byte> destination = stackalloc byte[source.Length];

        for (var i = 0; i < destination.Length; i++)
        {
            destination[i] = (byte)(source[i] & 0x7F);
        }

        return SHA256.HashData(destination);
    }
}