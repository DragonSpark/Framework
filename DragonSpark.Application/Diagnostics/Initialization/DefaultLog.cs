using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.Diagnostics.Initialization;

public sealed class DefaultLog<T> : DragonSpark.Model.Results.Instance<ILogger>
{
	public static DefaultLog<T> Default { get; } = new();

	DefaultLog() : base(LoggerFactory.Create(x => x.AddDebug().AddConsole()).CreateLogger<T>()) {}
}