using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Runtime;
using Serilog;

namespace DragonSpark.Diagnostics;

sealed class FlushLogging : Command, IFlushLogging
{
    public FlushLogging(ILogger logger) : base(DisposeAny.Default.Then().Bind(logger)) {}
}