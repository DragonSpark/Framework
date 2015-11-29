using DragonSpark.Diagnostics;

namespace DragonSpark.Setup.Commands
{
	public class SetupLoggingCommand<TLoggingFacade> : SetupLoggingCommandBase where TLoggingFacade : ILogger, new()
	{
		protected override ILogger CreateLogger()
		{
			var result = new TLoggingFacade();
			return result;
		}
	}
}