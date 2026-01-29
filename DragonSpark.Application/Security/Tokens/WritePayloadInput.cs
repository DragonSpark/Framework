using System.Buffers;
using System.Net.Http;
using System.Text.Json;

namespace DragonSpark.Application.Security.Tokens;

public readonly record struct WritePayloadInput(
    HttpRequestMessage Message,
    string? Token,
    Utf8JsonWriter Writer,
    ArrayBufferWriter<byte> Buffer);