using DragonSpark.Model.Results;
using Serilog;

namespace DragonSpark.Diagnostics;

public sealed class CurrentLogger : Variable<ILogger>
{
    public static CurrentLogger Default { get; } = new();

    CurrentLogger() {}
}