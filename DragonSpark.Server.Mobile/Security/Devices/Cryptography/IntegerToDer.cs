using System;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences.Memory;
using NetFabric.Hyperlinq;

namespace DragonSpark.Server.Mobile.Security.Devices.Cryptography;

sealed class IntegerToDer : ISelect<ReadOnlyMemory<byte>, Lease<byte>>
{
    public static IntegerToDer Default { get; } = new();

    IntegerToDer() : this(NewLeasing<byte>.Default) {}

    readonly INewLeasing<byte> _new;

    public IntegerToDer(INewLeasing<byte> @new) => _new = @new;

    public Lease<byte> Get(ReadOnlyMemory<byte> parameter)
    {
        // INTEGER 0 → 02 01 00
        if (parameter.Length == 0)
        {
            var zero = _new.Get(3);
            zero.Store[0] = 0x02; // INTEGER tag
            zero.Store[1] = 0x01; // length = 1
            zero.Store[2] = 0x00; // value = 0
            return zero.AsEnumerable();
        }

        var span = parameter.Span;

        // Trim leading zeros but leave at least one byte
        var i = 0;
        while (i < span.Length - 1 && span[i] == 0x00)
        {
            i++;
        }

        // Need a leading 0x00 if MSB set to keep it positive per DER
        var needZero       = (span[i] & 0x80) != 0;
        var significantLen = span.Length - i;
        var contentLen     = (needZero ? 1 : 0) + significantLen;

        // Short-form length OK for ECDSA (<=33). For >127, implement long-form.
        var result = _new.Get(2 + contentLen);

        // Tag + length
        result.Store[0] = 0x02;
        result.Store[1] = (byte)contentLen;

        var content = result.Store.AsSpan(2, contentLen);
        if (needZero)
        {
            content[0] = 0x00;
            span.Slice(i, significantLen).CopyTo(content[1..]);
        }
        else
        {
            span.Slice(i, significantLen).CopyTo(content);
        }

        return result.AsEnumerable();
    }
}