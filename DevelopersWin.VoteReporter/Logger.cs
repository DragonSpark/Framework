using DragonSpark.Windows;
using DragonSpark.Windows.Diagnostics;

namespace DevelopersWin.VoteReporter
{
	public class Logger : CompositeLoggerFacade
	{
		public Logger() : base( new TextLogger(), new TraceLogger() )
		{}
	}
}
