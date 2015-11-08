using DragonSpark.Windows;
using DragonSpark.Windows.Logging;

namespace DevelopersWin.VoteReporter
{
	public class Logger : CompositeLoggerFacade
	{
		public Logger() : base( new TextLogger(), new TraceLogger() )
		{}
	}
}
