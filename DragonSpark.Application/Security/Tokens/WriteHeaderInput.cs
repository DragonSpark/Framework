using System.Buffers;
using System.Text.Json;

namespace DragonSpark.Application.Security.Tokens;

public readonly record struct WriteHeaderInput(
    DPoPHeader Subject,
    Utf8JsonWriter Writer,
    ArrayBufferWriter<byte> Buffer);