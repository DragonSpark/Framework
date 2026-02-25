using System;
using System.Buffers.Binary;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Assertion;

sealed class DetermineCount : ISelect<Array<byte>, uint?>
{
    public static DetermineCount Default { get; } = new();

    DetermineCount() : this(AuthenticationDataLength.Default) {}

    readonly byte _length;

    public DetermineCount(byte length) => _length = length;

    public uint? Get(Array<byte> parameter)
        => parameter.Length == _length
               ? BinaryPrimitives.ReadUInt32BigEndian(parameter.Open().AsSpan(33, 4))
               : null;
}