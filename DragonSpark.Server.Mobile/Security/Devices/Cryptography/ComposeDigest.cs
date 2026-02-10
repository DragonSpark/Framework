using System;
using System.Security.Cryptography;
using System.Text;
using DragonSpark.Model.Selection;

namespace DragonSpark.Server.Mobile.Security.Devices.Cryptography;

sealed class ComposeDigest : ISelect<ReadOnlyMemory<char>, ReadOnlyMemory<byte>>
{
    public static ComposeDigest Default { get; } = new();

    ComposeDigest() : this(Encoding.UTF8) {}

    readonly Encoding _encoding;

    public ComposeDigest(Encoding encoding) => _encoding = encoding;

    public ReadOnlyMemory<byte> Get(ReadOnlyMemory<char> parameter)
    {
        var        from    = parameter.Span;
        Span<byte> to      = stackalloc byte[from.Length * 3];
        var        written = _encoding.GetBytes(from, to);
        return SHA256.HashData(to[..written]);
    }
}