using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.Diagnostics.Initialization;

public sealed class DefaultInitializeLog<T> : DragonSpark.Model.Results.Instance<ILogger>
{
	public static DefaultInitializeLog<T> Default { get; } = new DefaultInitializeLog<T>();

	DefaultInitializeLog() : base(LoggerFactory.Create(x => x.AddDebug()).CreateLogger<T>()) {}
}