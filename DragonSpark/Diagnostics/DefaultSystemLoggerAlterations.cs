using DragonSpark.Extensions;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using Serilog;
using System;

namespace DragonSpark.Diagnostics
{
	public sealed class DefaultSystemLoggerAlterations : ItemSource<IAlteration<LoggerConfiguration>>
	{
		public static DefaultSystemLoggerAlterations Default { get; } = new DefaultSystemLoggerAlterations();
		DefaultSystemLoggerAlterations() : base( HistoryAlteration.Implementation.Append( DefaultLoggerAlterations.Default ) ) {}

		sealed class HistoryAlteration : AlterationBase<LoggerConfiguration>
		{
			public static HistoryAlteration Implementation { get; } = new HistoryAlteration();
			HistoryAlteration() : this( LoggingHistory.Default.Get ) {}

			readonly Func<ILoggerHistory> history;

			HistoryAlteration( Func<ILoggerHistory> history )
			{
				this.history = history;
			}

			public override LoggerConfiguration Get( LoggerConfiguration parameter ) => parameter.WriteTo.Sink( history() );
		}
	}
}