using System.Buffers.Text;
using DragonSpark.Model.Sequences.Memory;
using JetBrains.Annotations;

namespace DragonSpark.Application.Security.Tokens;

public sealed class Base64UrlMemoryEncoder : ILease<ReadOnlyMemory<byte>, byte>
{
    public static Base64UrlMemoryEncoder Default { get; } = new();

    Base64UrlMemoryEncoder() : this(NewLeasing<byte>.Default) {}

    readonly INewLeasing<byte> _leasing;

    public Base64UrlMemoryEncoder(INewLeasing<byte> leasing) => _leasing = leasing;

    [MustDisposeResource]
    public Leasing<byte> Get(ReadOnlyMemory<byte> parameter)
    {
        var from   = parameter.Span;
        var length = (from.Length + 2) / 3 * 4;
        var lease  = _leasing.Get((uint)length);
        var to     = lease.AsSpan();

        Span<byte> temp = stackalloc byte[length];

        Base64.EncodeToUtf8(from, temp, out _, out var written);

        temp[..written].CopyTo(to);

        for (var i = 0; i < written; i++)
        {
            to[i] = to[i] switch
            {
                (byte)'+' => (byte)'-',
                (byte)'/' => (byte)'_',
                _ => to[i]
            };
        }

        // Trim '=' padding
        var trimmed = written;
        while (trimmed > 0 && to[trimmed - 1] == (byte)'=')
        {
            trimmed--;
        }

        return lease.Size(trimmed);
    }
}