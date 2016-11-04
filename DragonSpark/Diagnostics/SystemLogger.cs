using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Scopes;
using Serilog;

namespace DragonSpark.Diagnostics
{
	public sealed class SystemLogger : SingletonScope<ILogger>
	{
		public static IScope<ILogger> Default { get; } = new SystemLogger();
		SystemLogger() : base( new LoggerFactory( DefaultSystemLoggerAlterations.Default ).GetDefault ) {}
	}
}