using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Scopes;
using Serilog;
using System;

namespace DragonSpark.Diagnostics
{
	public sealed class SystemLogger : SingletonScope<ILogger>
	{
		public static IScope<ILogger> Default { get; } = new SystemLogger();
		SystemLogger() : base( new Func<ILogger>( new LoggerFactory( DefaultSystemLoggerAlterations.Default.Get() ).Get ) ) {}
	}
}