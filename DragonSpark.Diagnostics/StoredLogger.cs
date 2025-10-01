using DragonSpark.Model.Results;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace DragonSpark.Diagnostics;

sealed class StoredLogger : Stored<ILogger>
{
    public StoredLogger(IConfiguration configuration) : this(CurrentLogger.Default, configuration) {}

    public StoredLogger(IMutable<ILogger?> store, IConfiguration configuration)
        : base(store, new CreateLogger(configuration)) {}
}