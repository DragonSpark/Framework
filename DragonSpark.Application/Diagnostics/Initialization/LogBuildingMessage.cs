using DragonSpark.Diagnostics.Logging;

namespace DragonSpark.Application.Diagnostics.Initialization;

sealed class LogBuildingMessage<T> : LogMessage<string>
{
	public static LogBuildingMessage<T> Default { get; } = new();

	LogBuildingMessage() : base(DefaultInitializeLog<T>.Default.Get(), "{Program} is building...") {}
}