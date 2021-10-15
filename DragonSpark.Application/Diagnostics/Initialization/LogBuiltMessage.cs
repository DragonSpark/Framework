using DragonSpark.Diagnostics.Logging;

namespace DragonSpark.Application.Diagnostics.Initialization;

sealed class LogBuiltMessage<T> : LogMessage<string>
{
	public static LogBuiltMessage<T> Default { get; } = new();

	LogBuiltMessage() : base(DefaultInitializeLog<T>.Default.Get(), "{Program} has built") {}
}