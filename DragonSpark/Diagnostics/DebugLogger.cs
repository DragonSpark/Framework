using DragonSpark.Runtime;
using System.Diagnostics;

namespace DragonSpark.Diagnostics
{
	public class DebugLogger : LoggerBase
	{
		public static DebugLogger Instance { get; } = new DebugLogger();

		public DebugLogger() : this( ExceptionFormatter.Instance, CurrentTime.Instance )
		{}

		public DebugLogger( IExceptionFormatter formatter, ICurrentTime time ) : base( formatter, time )
		{}

		protected override void Write( Line line )
		{
			Debug.WriteLine( line.Message );
		}
	}
}
