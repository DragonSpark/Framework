using Microsoft.Extensions.Logging;
using System;

namespace DragonSpark.Application.Diagnostics;

public readonly record struct LogExceptionInput(ILogger Logger, Exception Exception);