using System;
using DragonSpark.Model.Sequences.Memory;

namespace DragonSpark.Application.Security.Tokens;

sealed class WriteHeader : ILease<WriteHeaderInput, char>
{
    public static WriteHeader Default { get; } = new();

    WriteHeader() : this(MemoryTokenFormatter.Default, Base64UrlEncoder.Default) {}

    readonly ILease<ReadOnlyMemory<char>, char> _formatter;
    readonly ILease<ReadOnlyMemory<byte>, char> _encode;

    public WriteHeader(ILease<ReadOnlyMemory<char>, char> formatter, ILease<ReadOnlyMemory<byte>, char> encode)
    {
        _formatter = formatter;
        _encode    = encode;
    }

    public Leasing<char> Get(WriteHeaderInput parameter)
    {
        var ((kty, crv, x, y), writer, buffer) = parameter;
        writer.WriteString("typ", "dpop+jwt");
        writer.WriteString("alg", "ES256");
        writer.WritePropertyName("jwk");
        writer.WriteStartObject();
        writer.WriteString("kty", kty);
        writer.WriteString("crv", crv);
        writer.WriteString("x", x);
        writer.WriteString("y", y);
        writer.WriteEndObject();

        using var start  = _encode.Get(buffer.WrittenMemory);
        var       result = _formatter.Get(start.AsMemory());
        return result;
    }
}