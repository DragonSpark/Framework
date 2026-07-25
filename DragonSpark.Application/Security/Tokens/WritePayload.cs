using System.Text.Json;
using DragonSpark.Model.Sequences.Memory;

namespace DragonSpark.Application.Security.Tokens;

sealed class WritePayload : ILease<WritePayloadInput, char>
{
    public static WritePayload Default { get; } = new();

    WritePayload() : this(MemoryTokenFormatter.Default, Base64UrlCharacterEncoder.Default) {}

    readonly ILease<ReadOnlyMemory<char>, char> _formatter;
    readonly ILease<ReadOnlyMemory<byte>, char> _encode;

    public WritePayload(ILease<ReadOnlyMemory<char>, char> formatter, ILease<ReadOnlyMemory<byte>, char> encode)
    {
        _formatter = formatter;
        _encode    = encode;
    }

    public Leasing<char> Get(WritePayloadInput parameter)
    {
        var (message, token, buffer) = parameter;

        using var writer = new Utf8JsonWriter(buffer);
        writer.WriteStartObject();
        writer.WriteString("htm", message.Method.Method);
        writer.WriteString("htu", message.RequestUri!.GetLeftPart(UriPartial.Path));
        writer.WriteNumber("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds());

        if (token is { Length: > 0 })
        {
            writer.WriteString("nonce", token);
        }
        writer.WriteEndObject();
        writer.Flush();
        using var start  = _encode.Get(buffer.WrittenMemory);
        var       result = _formatter.Get(start.AsMemory());
        return result;
    }
}