using DragonSpark.Extensions;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using Serilog;
using System;

namespace DragonSpark.Diagnostics
{
	public sealed class DefaultSystemLoggerConfigurations : ItemSource<IAlteration<LoggerConfiguration>>
	{
		public static DefaultSystemLoggerConfigurations Default { get; } = new DefaultSystemLoggerConfigurations();
		DefaultSystemLoggerConfigurations() : base( HistoryAlteration.DefaultNested.Append( DefaultLoggerAlterations.Default.Get().AsEnumerable() ) ) {}

		sealed class HistoryAlteration : AlterationBase<LoggerConfiguration>
		{
			public static HistoryAlteration DefaultNested { get; } = new HistoryAlteration();
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