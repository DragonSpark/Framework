using Serilog.Events;
using System;

namespace DragonSpark.Diagnostics.Logging
{
	static class Implementations
	{
		public static Func<LogEvent, IScalar> Scalars { get; } = Logging.Scalars.Default.Stores().New().Get;

		public static Func<LogEvent, LogEvent> Projections { get; }
			= ProjectionLogEvents.Default.Stores().New().Get;
	}
}