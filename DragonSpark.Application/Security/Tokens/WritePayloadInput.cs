using System.Buffers;

namespace DragonSpark.Application.Security.Tokens;

public readonly record struct WritePayloadInput(
    HttpRequestMessage Message,
    string? Token,
    ArrayBufferWriter<byte> Buffer);