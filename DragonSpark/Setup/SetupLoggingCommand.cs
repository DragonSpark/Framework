using DragonSpark.Logging;

namespace DragonSpark.Setup
{
	public class SetupLoggingCommand<TLoggingFacade> : SetupLoggingCommandBase where TLoggingFacade : ILoggerFacade, new()
	{
		protected override ILoggerFacade CreateLogger()
		{
			var result = new TLoggingFacade();
			return result;
		}
	}
}