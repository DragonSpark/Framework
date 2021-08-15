using DragonSpark.Diagnostics.Logging;
using System;

namespace DragonSpark.Application.Diagnostics.Initialization
{
	sealed class LogInitializingMessage<T> : LogMessage<string, Version, int>
	{
		public static LogInitializingMessage<T> Default { get; } = new LogInitializingMessage<T>();

		LogInitializingMessage() : base(DefaultInitializeLog<T>.Default.Get(),
		                                "{Program} {Version} is initializing with process {Process}") {}
	}
}