using System;
using System.Diagnostics;

namespace DragonSpark.Diagnostics
{
	public class Tracer : ITracer
	{
		readonly ILogger logger;

		public Tracer( ILogger logger )
		{
			this.logger = logger;
		}

		public void Trace( Action action, string message, Guid? id = null )
		{
			var associatedId = id ?? Guid.NewGuid();
			var start = Stopwatch.GetTimestamp();
			logger.StartTrace( message, associatedId );
			action();
			var time = TimeSpan.FromTicks( Stopwatch.GetTimestamp() - start );
			logger.EndTrace( message, associatedId, time );
		}
	}
}