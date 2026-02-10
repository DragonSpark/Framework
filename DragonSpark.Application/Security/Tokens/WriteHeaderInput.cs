using System.Buffers;

namespace DragonSpark.Application.Security.Tokens;

public readonly record struct WriteHeaderInput(JwkHeader Subject, ArrayBufferWriter<byte> Buffer);