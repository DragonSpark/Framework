using System.Security.Cryptography;
using DragonSpark.Compose;
using DragonSpark.Model.Sequences.Memory;
using NetFabric.Hyperlinq;

namespace DragonSpark.Application.Security.Tokens;

sealed class DerToJose : ILease<ReadOnlyMemory<byte>, byte>
{
    public static DerToJose Default { get; } = new();

    DerToJose() : this(32, NewLeasing<byte>.Default) {}

    readonly int               _part;
    readonly INewLeasing<byte> _new;

    public DerToJose(int part, INewLeasing<byte> @new)
    {
        _part = part;
        _new  = @new;
    }

    public Leasing<byte> Get(ReadOnlyMemory<byte> parameter)
    {
        int offset = 0;

        if (ReadByte() != 0x30) throw new CryptographicException("Invalid DER seq");
        _ = ReadLength();

        if (ReadByte() != 0x02) throw new CryptographicException("Invalid DER int r");
        var r = ReadInt();

        if (ReadByte() != 0x02) throw new CryptographicException("Invalid DER int s");
        var s = ReadInt();

        using var rPadded = LeftPad(r, _part);
        using var sPadded = LeftPad(s, _part);

        var result = _new.Get(_part * 2);
        var to     = result.AsSpan();
        rPadded.Memory.Span.CopyTo(to[.._part]);
        sPadded.Memory.Span.CopyTo(to.Slice(_part, _part));
        return result;

        byte ReadByte() => parameter.Span[offset++];

        ReadOnlySpan<byte> ReadSpan(int len)
        {
            var slice = parameter.Span.Slice(offset, len);
            offset += len;
            return slice;
        }

        int ReadLength()
        {
            int b = ReadByte();
            if (b >= 0x80)
            {
                var lenBytes = b & 0x7F;
                var len      = 0;
                for (var i = 0; i < lenBytes; i++)
                {
                    len = (len << 8) | ReadByte();
                }

                return len;
            }

            return b;
        }

        ReadOnlySpan<byte> ReadInt()
        {
            int len   = ReadByte();
            var slice = ReadSpan(len);
            return slice.Length > 0 && slice[0] == 0x00 ? slice[1..] : slice;
        }

        Lease<byte> LeftPad(ReadOnlySpan<byte> v, int size)
        {
            if (v.Length <= size)
            {
                var lease = _new.Get(size).AsEnumerable();
                var span  = lease.Memory.Span;

                v.CopyTo(span[(size - v.Length)..]);

                return lease;
            }

            throw new CryptographicException("Part too long");
        }
    }
}