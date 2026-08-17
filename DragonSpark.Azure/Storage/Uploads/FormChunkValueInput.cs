using Microsoft.AspNetCore.Http;

namespace DragonSpark.Azure.Storage.Uploads;

public readonly record struct FormChunkValueInput(IFormCollection Form, ushort? Index);