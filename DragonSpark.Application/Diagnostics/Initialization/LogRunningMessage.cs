using DragonSpark.Diagnostics.Logging;
using System;

namespace DragonSpark.Application.Diagnostics.Initialization;

sealed class LogRunningMessage<T> : LogMessage<string, Version, int>
{
	public static LogRunningMessage<T> Default { get; } = new LogRunningMessage<T>();

	LogRunningMessage() : base(DefaultInitializeLog<T>.Default.Get(),
	                           "{Program} {Version} is running on process {Process}") {}
}