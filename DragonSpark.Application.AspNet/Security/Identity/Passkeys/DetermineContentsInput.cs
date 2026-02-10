using System.IO;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

public readonly record struct DetermineContentsInput(Stream Stream, string? ContentType);