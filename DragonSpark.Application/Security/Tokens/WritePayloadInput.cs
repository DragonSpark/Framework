using System.Buffers;
using System.Net.Http;

namespace DragonSpark.Application.Security.Tokens;

public readonly record struct WritePayloadInput(
    HttpRequestMessage Message,
    string? Token,
    ArrayBufferWriter<byte> Buffer);