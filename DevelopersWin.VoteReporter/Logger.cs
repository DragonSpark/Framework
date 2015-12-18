using DragonSpark.Diagnostics;
using DragonSpark.Windows;
using DragonSpark.Windows.Diagnostics;

namespace DevelopersWin.VoteReporter
{
	public class Logger : CompositeMessageLogger
	{
		public Logger() : base( new TextMessageLogger(), new TraceMessageLogger() )
		{}
	}
}
