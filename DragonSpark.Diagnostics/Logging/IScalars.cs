using Serilog.Events;
using DragonSpark.Model.Selection;

namespace DragonSpark.Diagnostics.Logging
{
	public interface IScalars : ISelect<LogEvent, IScalar> {}
}