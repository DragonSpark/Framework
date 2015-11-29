using System;
using System.Collections.Generic;
using System.Linq;
using DragonSpark.Extensions;
using DragonSpark.Runtime;

namespace DragonSpark.Diagnostics
{
	public class RecordingLogger : LoggerBase, IRecordingLogger
	{
		readonly IList<Line> lines = new List<Line>();

		public RecordingLogger() : this( ExceptionFormatter.Instance, CurrentTime.Instance )
		{}

		public RecordingLogger( IExceptionFormatter formatter, ICurrentTime time ) : base( formatter, time )
		{}

		public void Playback( Action<string> write )
		{
			lines.OrderBy( x => x.Time ).Each( tuple => write( tuple.Message ) );
		}

		protected override void Write( Line line )
		{
			lines.Add( line );
		}

		public IEnumerable<Line> Lines => lines;
	}
}