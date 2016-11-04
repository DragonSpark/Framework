using DragonSpark.Runtime.Assignments;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Scopes;
using Serilog;
using System.Collections.Generic;

namespace DragonSpark.Diagnostics
{
	public sealed class LoggerAlterations : ItemsScope<IAlteration<LoggerConfiguration>>
	{
		public static LoggerAlterations Default { get; } = new LoggerAlterations();
		LoggerAlterations() : base( new SingletonScope<IEnumerable<IAlteration<LoggerConfiguration>>>( DefaultLoggerAlterations.Default.IncludeExports ) ) {}

		public sealed class Configure : AssignGlobalScopeCommand<IEnumerable<IAlteration<LoggerConfiguration>>>
		{
			public static Configure Implementation { get; } = new Configure();
			Configure() : base( Default.Scope ) {}
		}
	}
}