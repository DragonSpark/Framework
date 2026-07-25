using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.Diagnostics;

public readonly record struct LogExceptionInput(ILogger Logger, Exception Exception);