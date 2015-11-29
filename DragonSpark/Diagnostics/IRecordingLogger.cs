using System.Collections.Generic;

namespace DragonSpark.Diagnostics
{
	public interface IRecordingLogger : ILogger
	{
		IEnumerable<Line> Lines { get; }
	}
}