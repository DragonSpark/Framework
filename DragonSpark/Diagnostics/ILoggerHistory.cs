using Serilog.Core;
using Serilog.Events;
using System.Collections.Generic;

namespace DragonSpark.Diagnostics
{
	public interface ILoggerHistory : ILogEventSink
	{
		IEnumerable<LogEvent> Events { get; }

		void Clear();
	}
}