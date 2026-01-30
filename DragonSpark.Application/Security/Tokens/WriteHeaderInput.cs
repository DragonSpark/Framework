using System.Buffers;

namespace DragonSpark.Application.Security.Tokens;

public readonly record struct WriteHeaderInput(DPoPHeader Subject, ArrayBufferWriter<byte> Buffer);