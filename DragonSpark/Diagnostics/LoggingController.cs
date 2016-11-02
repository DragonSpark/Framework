using DragonSpark.Sources.Scopes;
using Serilog.Core;

namespace DragonSpark.Diagnostics
{
	public sealed class LoggingController : ScopedSingleton<LoggingLevelSwitch>
	{
		public static LoggingController Default { get; } = new LoggingController();
		LoggingController() : base( () => new LoggingLevelSwitch( MinimumLevelConfiguration.Default.Get() ) ) {}
	}
}