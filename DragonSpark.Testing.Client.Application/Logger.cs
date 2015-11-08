using DragonSpark.Application;
using Prism.Logging;

namespace DragonSpark.Testing.Client.Application
{
	public class Logger : CompositeLoggerFacade
	{
		public Logger() : base( new TraceLogger(), new TextLogger() )
		{}
	}
}
