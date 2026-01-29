using System;
using System.Buffers.Text;
using DragonSpark.Model.Sequences.Memory;
using JetBrains.Annotations;

namespace DragonSpark.Application.Security.Tokens;

public sealed class Base64UrlEncoder : ILease<ReadOnlyMemory<byte>, char>
{
    public static Base64UrlEncoder Default { get; } = new();

    Base64UrlEncoder() : this(NewLeasing<char>.Default) {}

    readonly INewLeasing<char> _leasing;

    public Base64UrlEncoder(INewLeasing<char> leasing) => _leasing = leasing;

    [MustDisposeResource]
    public Leasing<char> Get(ReadOnlyMemory<byte> parameter)
    {
        var from   = parameter.Span;
        var length = (from.Length + 2) / 3 * 4;

        // Lease char buffer
        var lease = _leasing.Get((uint)length);
        var to    = lease.AsSpan();

        // Encode into a temporary byte buffer
        Span<byte> temp = stackalloc byte[length];
        Base64.EncodeToUtf8(from, temp, out _, out var written);

        // Convert bytes → chars
        for (var i = 0; i < written; i++)
        {
            to[i] = (char)temp[i];
        }

        // URL-safe replacements
        to.Replace('+', '-');
        to.Replace('/', '_');

        // Trim '=' padding
        var trimmed = written;
        while (trimmed > 0 && to[trimmed - 1] == '=')
        {
            trimmed--;
        }

        return lease.Size(trimmed);
    }
}